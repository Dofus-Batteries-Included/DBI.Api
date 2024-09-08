using System.Text.Json;
using Microsoft.Extensions.Options;
using Server.Common.Workers;
using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues;
using Server.Domains.TreasureSolver.Services.I18N;
using Server.Domains.TreasureSolver.Services.Maps;
using Server.Infrastructure.Repository;

namespace Server.Domains.TreasureSolver.Workers;

public class RefreshGameData : PeriodicService
{
    readonly string _cacheDirectory;
    readonly string _cachedValuesFilePath;
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    readonly JsonSerializerOptions _cachedValuesJsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };
    readonly IHttpClientFactory _httpClientFactory;
    readonly LanguagesService _languagesService;
    readonly CluesService _cluesService;
    readonly MapsService _mapsService;
    readonly ILogger<PeriodicService> _logger;
    string? _lastGameVersion;

    public RefreshGameData(
        IHttpClientFactory httpClientFactory,
        LanguagesService languagesService,
        CluesService cluesService,
        MapsService mapsService,
        IOptions<RepositoryOptions> repositoryOptions,
        ILogger<PeriodicService> logger
    ) : base(TimeSpan.FromHours(1), logger)
    {
        _httpClientFactory = httpClientFactory;
        _languagesService = languagesService;
        _cluesService = cluesService;
        _mapsService = mapsService;
        _logger = logger;
        _cacheDirectory = repositoryOptions.Value.BasePath;
        _cachedValuesFilePath = Path.Join(_cacheDirectory, "refresh-game-state.json");
    }

    protected override async Task OnStartAsync(CancellationToken stoppingToken)
    {
        RefreshGameDataCache? cachedState = await ReadRefreshGameCacheAsync(stoppingToken);
        if (cachedState == null || string.IsNullOrWhiteSpace(cachedState.LastGameVersion))
        {
            return;
        }

        I18NFile? cachedFrTable = await ReadI18NCachedFileAsync(cachedState.LastGameVersion, Language.Fr, stoppingToken);
        if (cachedFrTable != null)
        {
            _logger.LogInformation("Loading FR table.");
            _languagesService.French.SetTable(cachedFrTable.Entries);
        }

        I18NFile? cachedEnTable = await ReadI18NCachedFileAsync(cachedState.LastGameVersion, Language.En, stoppingToken);
        if (cachedEnTable != null)
        {
            _logger.LogInformation("Loading EN table.");
            _languagesService.English.SetTable(cachedEnTable.Entries);
        }

        I18NFile? cachedEsTable = await ReadI18NCachedFileAsync(cachedState.LastGameVersion, Language.Es, stoppingToken);
        if (cachedEsTable != null)
        {
            _logger.LogInformation("Loading ES table.");
            _languagesService.Spanish.SetTable(cachedEsTable.Entries);
        }

        I18NFile? cachedDeTable = await ReadI18NCachedFileAsync(cachedState.LastGameVersion, Language.De, stoppingToken);
        if (cachedDeTable != null)
        {
            _logger.LogInformation("Loading DE table.");
            _languagesService.German.SetTable(cachedDeTable.Entries);
        }

        I18NFile? cachedPtTable = await ReadI18NCachedFileAsync(cachedState.LastGameVersion, Language.Pt, stoppingToken);
        if (cachedPtTable != null)
        {
            _logger.LogInformation("Loading PT table.");
            _languagesService.Portuguese.SetTable(cachedPtTable.Entries);
        }

        Dictionary<int, Clue>? cachedClues = await ReadCluesCachedFile(cachedState.LastGameVersion, stoppingToken);
        if (cachedClues != null)
        {
            _logger.LogInformation("Loading clues.");
            _cluesService.SaveClues(cachedClues);
        }

        Dictionary<long, Map>? cachedMaps = await ReadMapsCachedFile(cachedState.LastGameVersion, stoppingToken);
        if (cachedMaps != null)
        {
            _logger.LogInformation("Loading maps.");
            _mapsService.SaveMaps(cachedMaps);
        }
    }

    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Starting refresh of game data.");

        string version = await GetLatestVersionAsync(stoppingToken);
        await WriteRefreshGameCacheAsync(new RefreshGameDataCache { LastGameVersion = version }, stoppingToken);

        if (version == _lastGameVersion)
        {
            Logger.LogInformation("Latest game version has not changed: {Version}.", version);
            return;
        }

        await RefreshLanguages(version, stoppingToken);
        await RefreshCluesAsync(version, stoppingToken);
        await RefreshMapsAsync(version, stoppingToken);

        _lastGameVersion = version;

        Logger.LogInformation("Done refreshing game data.");
    }

    async Task<string> GetLatestVersionAsync(CancellationToken stoppingToken)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        DdcVersionsClient client = new(httpClient);
        GetAvailableVersionsResponse? response = await client.GetAvailableVersionsAsync(stoppingToken);
        if (response == null)
        {
            throw new InvalidOperationException("Could not find latest version.");
        }

        return response.Latest;
    }

    async Task RefreshLanguages(string version, CancellationToken stoppingToken)
    {
        Dictionary<int, string> frTable = await GetLanguage(version, Language.Fr, stoppingToken);
        _languagesService.French.SetTable(frTable);

        Dictionary<int, string> enTable = await GetLanguage(version, Language.En, stoppingToken);
        _languagesService.English.SetTable(enTable);

        Dictionary<int, string> esTable = await GetLanguage(version, Language.Es, stoppingToken);
        _languagesService.Spanish.SetTable(esTable);

        Dictionary<int, string> deTable = await GetLanguage(version, Language.De, stoppingToken);
        _languagesService.German.SetTable(deTable);

        Dictionary<int, string> ptTable = await GetLanguage(version, Language.Pt, stoppingToken);
        _languagesService.Portuguese.SetTable(ptTable);
    }

    async Task RefreshCluesAsync(string version, CancellationToken stoppingToken)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        DdcRawDataClient client = new(httpClient);
        FileResponse? response = await client.GetPointsOfInterestAsync(version, stoppingToken);
        if (response == null)
        {
            throw new InvalidOperationException($"Could not download clues data in version {version}.");
        }

        PointOfInterest[]? pois = await JsonSerializer.DeserializeAsync<PointOfInterest[]>(response.Stream, _jsonSerializerOptions, stoppingToken);
        if (pois == null)
        {
            throw new InvalidOperationException($"Could not parse clues data in version {version}.");
        }

        Dictionary<int, Clue> clues = pois.ToDictionary(
            p => p.PoiId,
            p => new Clue
            {
                ClueId = p.PoiId,
                Name = _languagesService.Get(p.NameId)
            }
        );

        Logger.LogInformation("Found a total of {Count} clues.", clues.Count);

        await WriteCluesCachedFileAsync(version, clues, stoppingToken);

        _cluesService.SaveClues(clues);
    }

    async Task RefreshMapsAsync(string version, CancellationToken stoppingToken)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        DdcRawDataClient client = new(httpClient);
        FileResponse? response = await client.GetMapPositionsAsync(version, stoppingToken);
        if (response == null)
        {
            throw new InvalidOperationException($"Could not download clues data in version {version}.");
        }

        MapPositions[]? mapPositions = await JsonSerializer.DeserializeAsync<MapPositions[]>(response.Stream, _jsonSerializerOptions, stoppingToken);
        if (mapPositions == null)
        {
            throw new InvalidOperationException($"Could not parse clues data in version {version}.");
        }

        Dictionary<long, Map> maps = mapPositions.ToDictionary(
            m => m.MapId,
            p => new Map
            {
                MapId = p.MapId,
                PosX = p.PosX,
                PosY = p.PosY,
                WorldMap = p.WorldMap
            }
        );

        Logger.LogInformation("Found a total of {Count} maps.", maps.Count);

        await WriteMapsCachedFileAsync(version, maps, stoppingToken);

        _mapsService.SaveMaps(maps);
    }

    async Task<Dictionary<int, string>> GetLanguage(string version, Language language, CancellationToken stoppingToken)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient();
        DdcRawDataClient client = new(httpClient);
        FileResponse? response = await client.GetI18NAsync(language, version, stoppingToken);
        if (response == null)
        {
            throw new InvalidOperationException($"Could not download data for language {language} in version {version}.");
        }

        I18NFile? i18NFile = await JsonSerializer.DeserializeAsync<I18NFile>(response.Stream, _jsonSerializerOptions, stoppingToken);
        if (i18NFile == null)
        {
            throw new InvalidOperationException($"Could not parse data for language {language} in version {version}.");
        }

        await WriteI18NCachedFileAsync(version, language, i18NFile, stoppingToken);

        return i18NFile.Entries;
    }

    async Task<RefreshGameDataCache?> ReadRefreshGameCacheAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_cachedValuesFilePath))
        {
            return null;
        }

        await using FileStream stream = File.OpenRead(_cachedValuesFilePath);
        RefreshGameDataCache? result = await JsonSerializer.DeserializeAsync<RefreshGameDataCache>(stream, _cachedValuesJsonSerializerOptions, cancellationToken);

        if (result != null)
        {
            _logger.LogInformation("Found cached version {Version} at {Path}.", result.LastGameVersion, _cachedValuesFilePath);
        }

        return result;
    }

    async Task WriteRefreshGameCacheAsync(RefreshGameDataCache cache, CancellationToken cancellationToken)
    {
        string? directory = Path.GetDirectoryName(_cachedValuesFilePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using FileStream stream = File.OpenWrite(_cachedValuesFilePath);
        await JsonSerializer.SerializeAsync(stream, cache, _cachedValuesJsonSerializerOptions, cancellationToken);

        _logger.LogInformation("Written cached version {Version} at {Path}.", cache.LastGameVersion, _cachedValuesFilePath);
    }

    async Task<I18NFile?> ReadI18NCachedFileAsync(string version, Language language, CancellationToken cancellationToken)
    {
        string path = Path.Join(_cacheDirectory, version, $"{language.ToString().ToLowerInvariant()}.i18n.json");
        if (!File.Exists(path))
        {
            return null;
        }

        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<I18NFile>(stream, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    async Task WriteI18NCachedFileAsync(string version, Language language, I18NFile file, CancellationToken cancellationToken)
    {
        string directory = Path.Join(_cacheDirectory, version);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Join(directory, $"{language.ToString().ToLowerInvariant()}.i18n.json");
        await using FileStream stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, file, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    async Task<Dictionary<int, Clue>?> ReadCluesCachedFile(string version, CancellationToken cancellationToken)
    {
        string path = Path.Join(_cacheDirectory, version, "clues.json");
        if (!File.Exists(path))
        {
            return null;
        }

        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<Dictionary<int, Clue>>(stream, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    async Task WriteCluesCachedFileAsync(string version, Dictionary<int, Clue> file, CancellationToken cancellationToken)
    {
        string directory = Path.Join(_cacheDirectory, version);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Join(directory, "clues.json");
        await using FileStream stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, file, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    async Task<Dictionary<long, Map>?> ReadMapsCachedFile(string version, CancellationToken cancellationToken)
    {
        string path = Path.Join(_cacheDirectory, version, "maps.json");
        if (!File.Exists(path))
        {
            return null;
        }

        await using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<Dictionary<long, Map>>(stream, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    async Task WriteMapsCachedFileAsync(string version, Dictionary<long, Map> file, CancellationToken cancellationToken)
    {
        string directory = Path.Join(_cacheDirectory, version);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Join(directory, "maps.json");
        await using FileStream stream = File.OpenWrite(path);
        await JsonSerializer.SerializeAsync(stream, file, _cachedValuesJsonSerializerOptions, cancellationToken);
    }

    class RefreshGameDataCache
    {
        public required string LastGameVersion { get; init; }
    }

    class I18NFile
    {
        public required string LanguageCode { get; init; }
        public required Dictionary<int, string> Entries { get; init; }
    }

    class PointOfInterest
    {
        public required int PoiId { get; init; }
        public required int NameId { get; init; }
    }

    class MapPositions
    {
        public required long MapId { get; init; }
        public required int PosX { get; init; }
        public required int PosY { get; init; }
        public required int WorldMap { get; init; }
    }
}
