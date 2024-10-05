using DBI.Server.Features.DataCenter.Raw.Models;

namespace DBI.Server.Features.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawAreasService(IReadOnlyCollection<RawArea> subAreas)
{
    readonly Dictionary<int, RawArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawArea? GetArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawArea> GetAreas() => _subAreas.Values;
}
