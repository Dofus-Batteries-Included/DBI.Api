using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawSuperAreasService(IReadOnlyCollection<RawSuperArea> superAreas)
{
    readonly Dictionary<int, RawSuperArea> _superAreas = superAreas.ToDictionary(superArea => superArea.Id, superArea => superArea);

    public RawSuperArea? GetSuperArea(int superAreaId) => _superAreas.GetValueOrDefault(superAreaId);
    public IEnumerable<RawSuperArea> GetSuperAreas() => _superAreas.Values;
}
