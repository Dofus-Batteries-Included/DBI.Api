using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldMapsService(IReadOnlyCollection<RawWorldMap> subAreas)
{
    readonly Dictionary<int, RawWorldMap> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawWorldMap? GetWorldMap(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawWorldMap> GetWorldMaps() => _subAreas.Values;
}
