using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawAreasService(IReadOnlyCollection<RawArea> areas)
{
    readonly Dictionary<int, RawArea> _areas = areas.ToDictionary(area => area.Id, area => area);

    public RawArea? GetArea(int area) => _areas.GetValueOrDefault(area);
    public IEnumerable<RawArea> GetAreas() => _areas.Values;
}
