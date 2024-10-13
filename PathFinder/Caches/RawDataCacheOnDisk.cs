using System.IO.Compression;
using DBI.DataCenter.Raw.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Caches;

public class RawDataCacheOnDisk : IRawDataCache
{
    readonly string _folder;
    readonly ILogger _logger;

    public RawDataCacheOnDisk(string folder, ILogger? logger = null)
    {
        _folder = folder;
        _logger = logger ?? NullLogger.Instance;
    }

    public Task<bool> ContainsDataAsync(RawDataType rawDataType, CancellationToken cancellationToken = default)
    {
        string path = GetFilePath(rawDataType);
        return Task.FromResult(File.Exists(path));
    }

    public Task<Stream?> LoadDataAsync(RawDataType rawDataType, CancellationToken cancellationToken = default)
    {
        string path = GetFilePath(rawDataType);
        if (!File.Exists(path))
        {
            return Task.FromResult<Stream?>(null);
        }

        FileStream fileStream = File.OpenRead(path);
        BrotliStream uncompressedStream = new(fileStream, CompressionMode.Decompress, false);

        return Task.FromResult<Stream?>(uncompressedStream);
    }

    public async Task SaveDataAsync(RawDataType rawDataType, Stream stream, CancellationToken cancellationToken = default)
    {
        string path = GetFilePath(rawDataType);
        string? directory = Path.GetDirectoryName(path);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using FileStream fileStream = File.Open(path, FileMode.Create);
        await using BrotliStream compressedStream = new(fileStream, CompressionLevel.Optimal);

        _logger.LogInformation("Saving data for {Type} at {Path}.", rawDataType, path);

        await stream.CopyToAsync(compressedStream, cancellationToken);
    }

    string GetFilePath(RawDataType type) => Path.Join(_folder, GetFilename(type));

    static string GetFilename(RawDataType type) =>
        type switch
        {
            RawDataType.I18NFr => "fr.i18n.bin",
            RawDataType.I18NEn => "en.i18n.bin",
            RawDataType.I18NEs => "es.i18n.bin",
            RawDataType.I18NDe => "de.i18n.bin",
            RawDataType.I18NPt => "pt.i18n.bin",
            RawDataType.MapPositions => "map-positions.bin",
            RawDataType.MapCoordinates => "map-coordinates.bin",
            RawDataType.PointOfInterest => "point-of-interest.bin",
            RawDataType.WorldGraph => "world-graph.bin",
            RawDataType.SuperAreas => "super-areas.bin",
            RawDataType.Areas => "areas.bin",
            RawDataType.SubAreas => "sub-areas.bin",
            RawDataType.Maps => "maps.bin",
            RawDataType.WorldMaps => "world-maps.bin",
            RawDataType.Items => "items.bin",
            RawDataType.ItemSets => "item-sets.bin",
            RawDataType.ItemTypes => "item-types.bin",
            RawDataType.ItemSuperTypes => "item-super-types.bin",
            RawDataType.EvolutiveItemTypes => "evolutive-item-types.bin",
            RawDataType.Effects => "effects.bin",
            RawDataType.Recipes => "recipes.bin",
            RawDataType.Jobs => "jobs.bin",
            RawDataType.SkillNames => "skill-names.bin",
            RawDataType.Monsters => "monsters.bin",
            RawDataType.MonsterRaces => "monster-races.bin",
            RawDataType.MonsterSuperRaces => "monster-super-races.bin",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}
