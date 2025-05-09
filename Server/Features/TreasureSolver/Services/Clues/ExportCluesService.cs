﻿using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.PointOfInterests;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;
using DBI.Server.Infrastructure;
using Microsoft.Extensions.Options;

namespace DBI.Server.Features.TreasureSolver.Services.Clues;

/// <summary>
///     Export clue records.
/// </summary>
public class ExportCluesService
{
    readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    readonly FindCluesService _findCluesService;
    readonly LanguagesServiceFactory _languagesServiceFactory;
    readonly RawPointOfInterestsServiceFactory _rawPointOfInterestsServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly IEnumerable<IClueRecordsSource> _sources;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly IOptions<RepositoryOptions> _repositoryOptions;
    readonly ILogger<ExportCluesService> _logger;

    /// <summary>
    /// </summary>
    public ExportCluesService(
        FindCluesService findCluesService,
        LanguagesServiceFactory languagesServiceFactory,
        RawPointOfInterestsServiceFactory rawPointOfInterestsServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        IOptions<RepositoryOptions> repositoryOptions,
        ILogger<ExportCluesService> logger
    )
    {
        _findCluesService = findCluesService;
        _languagesServiceFactory = languagesServiceFactory;
        _rawPointOfInterestsServiceFactory = rawPointOfInterestsServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _logger = logger;
        _sources = sources;
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _repositoryOptions = repositoryOptions;
    }

    /// <summary>
    ///     Export clues
    /// </summary>
    public async Task<File> ExportCluesAsync(CancellationToken cancellationToken = default)
    {
        string version = Metadata.Version?.ToString() ?? "~dev";
        DateTime lastModificationDate = await _findCluesService.GetLastModificationDateAsync(cancellationToken) ?? DateTime.Now;

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

            await using FileStream writeStream = System.IO.File.Open(path, FileMode.Create);
            await JsonSerializer.SerializeAsync(writeStream, content, _serializerOptions, cancellationToken);

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
        LanguagesService languages = await _languagesServiceFactory.CreateLanguagesServiceAsync();
        RawPointOfInterestsService rawPointOfInterestsService = await _rawPointOfInterestsServiceFactory.CreateServiceAsync();
        return rawPointOfInterestsService.GetPointOfInterests()
            .Select(
                c => new FileClue
                {
                    ClueId = c.PoiId,
                    NameFr = languages.French?.Get(c.NameId),
                    NameEn = languages.English?.Get(c.NameId),
                    NameEs = languages.Spanish?.Get(c.NameId),
                    NameDe = languages.German?.Get(c.NameId),
                    NamePt = languages.Portuguese?.Get(c.NameId)
                }
            )
            .ToArray();
    }

    async Task<Dictionary<long, FileMap>> GetMapsAsync()
    {
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync();
        var maps = rawMapPositionsService.GetMapPositions().Select(m => new { m.MapId, m.PosX, m.PosY }).ToArray();
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

    /// <summary>
    ///     Representation of a file
    /// </summary>
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
