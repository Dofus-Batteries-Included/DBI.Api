using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawAreasService(IReadOnlyCollection<RawArea> subAreas)
{
    readonly Dictionary<int, RawArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawArea? GetArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawArea> GetAreas() => _subAreas.Values;
}
