using Server.Features.DataCenter.Raw.Models;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawWorldMapsService(IReadOnlyCollection<RawWorldMap> subAreas)
{
    readonly Dictionary<int, RawWorldMap> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawWorldMap? GetWorldMap(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawWorldMap> GetWorldMaps() => _subAreas.Values;
}
