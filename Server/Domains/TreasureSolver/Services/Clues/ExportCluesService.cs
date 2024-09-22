using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Server.Common.Models;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Services.I18N;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.PointOfInterests;
using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;
using Server.Infrastructure.Repository;

namespace Server.Domains.TreasureSolver.Services.Clues;

public class ExportCluesService
{
    readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    readonly FindCluesService _findCluesService;
    readonly LanguagesServiceFactory _languagesServiceFactory;
    readonly PointOfInterestsServiceFactory _pointOfInterestsServiceFactory;
    readonly MapsServiceFactory _mapsServiceFactory;
    readonly IEnumerable<IClueRecordsSource> _sources;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly IOptions<RepositoryOptions> _repositoryOptions;
    readonly ILogger<ExportCluesService> _logger;

    public ExportCluesService(
        FindCluesService findCluesService,
        LanguagesServiceFactory languagesServiceFactory,
        PointOfInterestsServiceFactory pointOfInterestsServiceFactory,
        MapsServiceFactory mapsServiceFactory,
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        IOptions<RepositoryOptions> repositoryOptions,
        ILogger<ExportCluesService> logger
    )
    {
        _findCluesService = findCluesService;
        _languagesServiceFactory = languagesServiceFactory;
        _pointOfInterestsServiceFactory = pointOfInterestsServiceFactory;
        _mapsServiceFactory = mapsServiceFactory;
        _logger = logger;
        _sources = sources;
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _repositoryOptions = repositoryOptions;
    }

    public async Task<File> ExportCluesAsync()
    {
        string version = Metadata.Version?.ToString() ?? "~dev";
        DateTime lastModificationDate = await _findCluesService.GetLastModificationDateAsync() ?? DateTime.Now;

        string filepath = GetExportedFilePath();
        string filename = GetExportedFileName(version, lastModificationDate);
        string path = Path.Join(filepath, filename);
        if (!System.IO.File.Exists(path))
        {
            _logger.LogInformation("Exporting clues...");

            FileContent content = new()
            {
                Version = version,
                LastModificationDate = lastModificationDate,
                Clues = await GetCluesAsync(),
                Maps = await GetMapsAsync()
            };

            int mapCount = content.Maps.Count;
            int cluesCount = content.Maps.Values.Sum(m => m.Clues.Length);

            _logger.LogInformation("Found a total of {CluesCount} clues in {MapsCount} maps.", cluesCount, mapCount);

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            await using FileStream writeStream = System.IO.File.OpenWrite(path);
            await JsonSerializer.SerializeAsync(writeStream, content, _serializerOptions);

            _logger.LogInformation("Saved export result at {Path}.", path);
        }
        else
        {
            _logger.LogInformation("Exported clues found at {Path}.", path);
        }

        FileStream stream = System.IO.File.OpenRead(path);
        return new File(filename, stream, "application/json");
    }

    static string GetExportedFileName(string version, DateTime lastModificationDate) => $"clues_{version}_{lastModificationDate:yyyy_MM_dd_HH_mm_ss}.json";
    string GetExportedFilePath() => Path.Join(_repositoryOptions.Value.BasePath, "Clues");

    async Task<FileClue[]> GetCluesAsync()
    {
        LanguagesService languages = await _languagesServiceFactory.CreateLanguagesService();
        PointOfInterestsService pointOfInterestsService = await _pointOfInterestsServiceFactory.CreateService();
        return pointOfInterestsService.GetPointOfInterests()
            .Select(
                c => new FileClue
                {
                    ClueId = c.PoiId,
                    NameFr = languages.French.Get(c.NameId),
                    NameEn = languages.English.Get(c.NameId),
                    NameEs = languages.Spanish.Get(c.NameId),
                    NameDe = languages.German.Get(c.NameId),
                    NamePt = languages.Portuguese.Get(c.NameId)
                }
            )
            .ToArray();
    }

    async Task<Dictionary<long, FileMap>> GetMapsAsync()
    {
        MapsService mapsService = await _mapsServiceFactory.CreateService();
        var maps = mapsService.GetMaps().Select(m => new { m.MapId, m.PosX, m.PosY }).ToArray();
        Dictionary<long, List<ClueRecord>> clues = new();
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> sourceData = await source.ExportData();
            foreach (KeyValuePair<long, IReadOnlyCollection<ClueRecord>> data in sourceData)
            {
                if (!clues.ContainsKey(data.Key))
                {
                    clues[data.Key] = [];
                }

                clues[data.Key].AddRange(data.Value);
            }
        }

        Dictionary<long, FileMap> result = new();
        foreach (var map in maps)
        {
            int[] cluesInMap = clues.TryGetValue(map.MapId, out List<ClueRecord>? clue)
                ? clue.GroupBy(c => c.ClueId).Select(g => g.OrderByDescending(c => c.RecordDate).First()).Select(c => c.ClueId).ToArray()
                : [];
            if (cluesInMap.Length == 0)
            {
                continue;
            }

            result[map.MapId] = new FileMap { Position = new Position(map.PosX, map.PosY), Clues = cluesInMap };
        }

        return result;
    }

    IClueRecordsSource[] GetDataSources() => _sources.Concat(_staticCluesDataSourcesService.GetDataSources()).ToArray();

    public record struct File(string Name, Stream Content, string Type);

    class FileContent
    {
        public required string Version { get; set; }
        public required DateTime LastModificationDate { get; set; }
        public required FileClue[] Clues { get; set; }
        public required Dictionary<long, FileMap> Maps { get; set; }
    }

    class FileClue
    {
        public required int ClueId { get; set; }
        public string? NameFr { get; set; }
        public string? NameEn { get; set; }
        public string? NameEs { get; set; }
        public string? NameDe { get; set; }
        public string? NamePt { get; set; }
    }

    class FileMap
    {
        public required Position Position { get; set; }
        public required int[] Clues { get; set; }
    }
}
