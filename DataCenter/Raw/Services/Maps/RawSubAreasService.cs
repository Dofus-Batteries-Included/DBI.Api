using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawSubAreasService(IReadOnlyCollection<RawSubArea> subAreas)
{
    readonly Dictionary<int, RawSubArea> _subAreas = subAreas.ToDictionary(subArea => subArea.Id, subArea => subArea);

    public RawSubArea? GetSubArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSubArea> GetSubAreas() => _subAreas.Values;
}
