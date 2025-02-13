﻿using System.Text.Json;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.PointOfInterests;
using DBI.Server.Common.Extensions;
using DBI.Server.Common.Workers;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;
using DBI.Server.Infrastructure;
using DBI.Server.Infrastructure.Database;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace DBI.Server.Features.TreasureSolver.Workers;

class RefreshDplnDataSource : PeriodicService
{
    readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };
    readonly IServiceScopeFactory _scopeFactory;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly ILogger<RefreshDplnDataSource> _logger;
    int? _lastHintsHashCode;
    int? _lastMapCluesHashCode;
    readonly string _cacheDirectory;
    readonly string _cacheFilePath;

    public RefreshDplnDataSource(
        IServiceScopeFactory scopeFactory,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        IOptions<RepositoryOptions> repositoryOptions,
        ILogger<RefreshDplnDataSource> logger
    ) : base(TimeSpan.FromHours(1), logger)
    {
        _scopeFactory = scopeFactory;
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _logger = logger;
        _cacheDirectory = Path.Join(repositoryOptions.Value.BasePath, "Clues", "Sources");
        _cacheFilePath = Path.Join(_cacheDirectory, "dpln.json");
    }

    public void TriggerRefreshAsap()
    {
        _lastHintsHashCode = null;
        _lastMapCluesHashCode = null;
        TriggerAsap();
    }

    protected override async Task OnStartAsync(CancellationToken stoppingToken)
    {
        CachedDplnDataSource? cached = await LoadCachedDataSourceAsync(stoppingToken);
        if (!cached.HasValue)
        {
            return;
        }

        _lastHintsHashCode = cached.Value.HintsHashCode;
        _lastMapCluesHashCode = cached.Value.MapCluesHashCode;
        StaticClueRecordsSource dataSource = new(cached.Value.Clues, cached.Value.LastModificationDate);
        _staticCluesDataSourcesService.RegisterDataSource(StaticCluesDataSourceName.Dpln, dataSource);
    }

    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Start refreshing DPLN data source...");

        (string hintsJson, string mapCluesJson) = await FetchDataFromWebsite(stoppingToken);

        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        int hintsHashCode = hintsJson.GetStableHashCode();
        int mapCluesHashCode = mapCluesJson.GetStableHashCode();
        if (_lastHintsHashCode == hintsHashCode && _lastMapCluesHashCode == mapCluesHashCode)
        {
            _logger.LogInformation("Nothing new, DPLN data source will not be refreshed.");
            return;
        }

        Hint[]? hints = JsonSerializer.Deserialize<Hint[]>(hintsJson, _serializerOptions);
        if (hints == null)
        {
            throw new InvalidOperationException("Could not deserialize hints.");
        }

        MapClues[]? mapClues = JsonSerializer.Deserialize<MapClues[]>(mapCluesJson, _serializerOptions);
        if (mapClues == null)
        {
            throw new InvalidOperationException("Could not deserialize map clues.");
        }

        DateTime now = DateTime.Now;
        Dictionary<long, IReadOnlyCollection<ClueRecord>> data = await TransformDataAsync(hints, mapClues, now, stoppingToken);

        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        int mapCount = data.Count;
        int cluesCount = data.Values.Sum(m => m.Count);
        _logger.LogInformation("Found a total of {CluesCount} clues in {MapsCount} maps in DPLN data.", cluesCount, mapCount);

        StaticClueRecordsSource dataSource = new(data, now);
        _staticCluesDataSourcesService.RegisterDataSource(StaticCluesDataSourceName.Dpln, dataSource);

        _lastHintsHashCode = hintsHashCode;
        _lastMapCluesHashCode = mapCluesHashCode;
        await SaveCachedDataSourceAsync(new CachedDplnDataSource(now, hintsHashCode, mapCluesHashCode, data));

        _logger.LogInformation("Done refreshing DPLN data source.");
    }

    async Task<Dictionary<long, IReadOnlyCollection<ClueRecord>>> TransformDataAsync(Hint[] hints, MapClues[] mapClues, DateTime date, CancellationToken stoppingToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        await using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        LanguagesServiceFactory languagesServiceFactory = scope.ServiceProvider.GetRequiredService<LanguagesServiceFactory>();
        RawPointOfInterestsServiceFactory rawPointOfInterestsServiceFactory = scope.ServiceProvider.GetRequiredService<RawPointOfInterestsServiceFactory>();
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory = scope.ServiceProvider.GetRequiredService<RawMapPositionsServiceFactory>();

        LanguagesService languagesService = await languagesServiceFactory.CreateLanguagesServiceAsync(cancellationToken: stoppingToken);
        RawPointOfInterestsService cluesService = await rawPointOfInterestsServiceFactory.CreateServiceAsync(cancellationToken: stoppingToken);
        RawMapPositionsService rawMapPositionsService = await rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: stoppingToken);

        RawPointOfInterest[] allPois = cluesService.GetPointOfInterests().ToArray();
        if (stoppingToken.IsCancellationRequested)
        {
            return [];
        }

        Dictionary<int, int> dplbClueToGameClueMapping = new();
        foreach (RawPointOfInterest poi in allPois)
        {
            string? nameFr = languagesService.French?.Get(poi.NameId);
            string? nameEn = languagesService.English?.Get(poi.NameId);
            string? nameEs = languagesService.Spanish?.Get(poi.NameId);

            string? nameFrWithoutAccent = nameFr?.RemoveAccents();
            string? nameEnWithoutAccent = nameEn?.RemoveAccents();
            string? nameEsWithoutAccent = nameEs?.RemoveAccents();

            Hint? hint = hints.FirstOrDefault(
                h => h.HintFr != null && h.HintFr == nameFrWithoutAccent
                     || h.HintEn != null && h.HintEn == nameEnWithoutAccent
                     || h.HintEs != null && h.HintEs == nameEsWithoutAccent
            );
            if (hint is null)
            {
                _logger.LogWarning("Could not find clue {Name} ({Id}) in DPLB file.", nameEn ?? nameFr ?? nameEs ?? "???", poi.PoiId);
                continue;
            }

            dplbClueToGameClueMapping[hint.ClueId] = poi.PoiId;
        }

        Dictionary<long, ClueRecord[]> result = new();
        foreach (MapClues clues in mapClues)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return [];
            }

            RawMapPosition[] maps = rawMapPositionsService.GetMapPositions().Where(m => m.PosX == clues.X && m.PosY == clues.Y).ToArray();
            int[] clueIds = clues.Clues.Where(c => dplbClueToGameClueMapping.ContainsKey(c)).Select(c => dplbClueToGameClueMapping[c]).ToArray();

            foreach (RawMapPosition map in maps)
            {
                result[map.MapId] = clueIds.Select(c => new ClueRecord { MapId = map.MapId, ClueId = c, RecordDate = date, Found = true }).ToArray();
            }
        }

        return result.ToDictionary(kv => kv.Key, IReadOnlyCollection<ClueRecord> (kv) => kv.Value);
    }

    async Task SaveCachedDataSourceAsync(CachedDplnDataSource cachedDplnDataSource)
    {
        if (!Directory.Exists(_cacheDirectory))
        {
            Directory.CreateDirectory(_cacheDirectory);
        }

        await using FileStream stream = File.Open(_cacheFilePath, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, cachedDplnDataSource, _serializerOptions);

        _logger.LogInformation("Saved DPLN data source at {Path}.", _cacheFilePath);
    }

    async Task<CachedDplnDataSource?> LoadCachedDataSourceAsync(CancellationToken stoppingToken)
    {
        if (!File.Exists(_cacheFilePath))
        {
            _logger.LogInformation("Could not find cached DPLN data source at {Path}.", _cacheFilePath);
            return null;
        }

        await using FileStream stream = File.OpenRead(_cacheFilePath);
        CachedDplnDataSource result = await JsonSerializer.DeserializeAsync<CachedDplnDataSource>(stream, _serializerOptions, stoppingToken);

        if (stoppingToken.IsCancellationRequested)
        {
            return null;
        }

        _logger.LogInformation("Loaded DPLN data source from {Path}.", _cacheFilePath);

        return result;
    }

    static async Task<(string HintsJson, string MapCluesJson)> FetchDataFromWebsite(CancellationToken stoppingToken)
    {
        BrowserFetcher browserFetcher = new();
        await browserFetcher.DownloadAsync();

        if (stoppingToken.IsCancellationRequested)
        {
            return (string.Empty, string.Empty);
        }

        await using IBrowser browser = await Puppeteer.LaunchAsync(
            new LaunchOptions
            {
                Args = ["--no-sandbox"],
                Headless = true
            }
        );

        if (stoppingToken.IsCancellationRequested)
        {
            return (string.Empty, string.Empty);
        }

        await using IPage page = await browser.NewPageAsync();

        if (stoppingToken.IsCancellationRequested)
        {
            return (string.Empty, string.Empty);
        }

        await page.GoToAsync("https://www.dofuspourlesnoobs.com/resolution-de-chasse-aux-tresors.html");

        if (stoppingToken.IsCancellationRequested)
        {
            return (string.Empty, string.Empty);
        }

        string cluesJson = await page.EvaluateExpressionAsync<string>("JSON.stringify(listHintId)");

        if (stoppingToken.IsCancellationRequested)
        {
            return (string.Empty, string.Empty);
        }

        string positionsJson = await page.EvaluateExpressionAsync<string>("JSON.stringify(listHuntClues)");

        return (cluesJson, positionsJson);
    }

    class Hint
    {
        public int ClueId { get; init; }
        public string? HintFr { get; init; }
        public string? HintEn { get; init; }
        public string? HintEs { get; init; }
    }

    class MapClues
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int[] Clues { get; init; } = [];
    }

    record struct CachedDplnDataSource(DateTime? LastModificationDate, int HintsHashCode, int MapCluesHashCode, IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> Clues);
}
