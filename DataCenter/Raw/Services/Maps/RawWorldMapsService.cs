using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawWorldMapsService(IReadOnlyCollection<RawWorldMap> worldMaps)
{
    readonly Dictionary<int, RawWorldMap> _worldMaps = worldMaps.ToDictionary(worldMap => worldMap.Id, worldMap => worldMap);

    public RawWorldMap? GetWorldMap(int worldMapId) => _worldMaps.GetValueOrDefault(worldMapId);
    public IEnumerable<RawWorldMap> GetWorldMaps() => _worldMaps.Values;
}
