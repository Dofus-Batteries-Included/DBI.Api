using Server.Features.DataCenter.Raw.Models;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawSubAreasService(IReadOnlyCollection<RawSubArea> subAreas)
{
    readonly Dictionary<int, RawSubArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawSubArea? GetSubArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSubArea> GetSubAreas() => _subAreas.Values;
}
