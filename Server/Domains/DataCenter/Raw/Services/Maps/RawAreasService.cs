using Server.Domains.DataCenter.Raw.Models;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawAreasService(IReadOnlyCollection<RawArea> subAreas)
{
    readonly Dictionary<int, RawArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawArea? GetArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawArea> GetAreas() => _subAreas.Values;
}
