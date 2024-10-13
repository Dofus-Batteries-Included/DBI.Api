﻿using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawSuperAreasService(IReadOnlyCollection<RawSuperArea> subAreas)
{
    readonly Dictionary<int, RawSuperArea> _subAreas = subAreas.ToDictionary(map => map.Id, map => map);

    public RawSuperArea? GetSuperArea(int subAreaId) => _subAreas.GetValueOrDefault(subAreaId);
    public IEnumerable<RawSuperArea> GetSuperAreas() => _subAreas.Values;
}
