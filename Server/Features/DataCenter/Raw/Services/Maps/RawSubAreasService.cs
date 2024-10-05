using DBI.Server.Features.DataCenter.Raw.Models;

namespace DBI.Server.Features.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawSubAreasService(IReadOnlyCollection<RawSubArea> subAreas)
{
    readonly Dictionary<int, RawSubArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawSubArea? GetSubArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSubArea> GetSubAreas() => _subAreas.Values;
}
