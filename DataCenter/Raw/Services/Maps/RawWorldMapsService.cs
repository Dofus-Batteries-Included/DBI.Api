using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawWorldMapsService(IReadOnlyCollection<RawWorldMap> subAreas)
{
    readonly Dictionary<int, RawWorldMap> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawWorldMap? GetWorldMap(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawWorldMap> GetWorldMaps() => _subAreas.Values;
}
