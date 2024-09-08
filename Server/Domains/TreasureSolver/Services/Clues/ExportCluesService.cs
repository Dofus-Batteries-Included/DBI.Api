using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;
using Server.Domains.TreasureSolver.Services.Maps;
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
    readonly ICluesService _cluesService;
    readonly IMapsService _mapsService;
    readonly IEnumerable<IClueRecordsSource> _sources;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly IOptions<RepositoryOptions> _repositoryOptions;
    readonly ILogger<ExportCluesService> _logger;

    public ExportCluesService(
        FindCluesService findCluesService,
        ICluesService cluesService,
        IMapsService mapsService,
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        IOptions<RepositoryOptions> repositoryOptions,
        ILogger<ExportCluesService> logger
    )
    {
        _findCluesService = findCluesService;
        _logger = logger;
        _cluesService = cluesService;
        _mapsService = mapsService;
        _sources = sources;
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _repositoryOptions = repositoryOptions;
    }

    public async Task<File> ExportCluesAsync()
    {
        string version = Metadata.Version?.ToString() ?? "~dev";
        DateTime lastModificationDate = await _findCluesService.GetLastModificationDate() ?? DateTime.Now;

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
                Clues = GetClues(),
                Maps = await GetMaps()
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

    FileClue[] GetClues() =>
        _cluesService.GetClues()
            .Select(
                c => new FileClue
                {
                    ClueId = c.ClueId,
                    NameFr = c.Name.French,
                    NameEn = c.Name.English,
                    NameEs = c.Name.Spanish,
                    NameDe = c.Name.German,
                    NamePt = c.Name.Portuguese
                }
            )
            .ToArray();

    async Task<Dictionary<long, FileMap>> GetMaps()
    {
        var maps = _mapsService.GetMaps().Select(m => new { m.MapId, m.PosX, m.PosY }).ToArray();
        Dictionary<long, List<ClueRecord>> clues = new();
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> sourceData = await source.ExportData();
            foreach (KeyValuePair<long, IReadOnlyCollection<ClueRecord>> data in sourceData)
            {
                if (!clues.ContainsKey(data.Key))
                {
                    clues[data.Key] = new List<ClueRecord>();
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
