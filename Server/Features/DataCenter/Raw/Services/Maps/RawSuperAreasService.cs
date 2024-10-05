using DBI.Server.Features.DataCenter.Raw.Models;

namespace DBI.Server.Features.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawSuperAreasService(IReadOnlyCollection<RawSuperArea> subAreas)
{
    readonly Dictionary<int, RawSuperArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawSuperArea? GetSuperArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSuperArea> GetSuperAreas() => _subAreas.Values;
}
