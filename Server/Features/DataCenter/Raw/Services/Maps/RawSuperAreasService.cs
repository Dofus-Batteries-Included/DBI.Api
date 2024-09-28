using Server.Features.DataCenter.Raw.Models;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawSuperAreasService(IReadOnlyCollection<RawSuperArea> subAreas)
{
    readonly Dictionary<int, RawSuperArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawSuperArea? GetSuperArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSuperArea> GetSuperAreas() => _subAreas.Values;
}
