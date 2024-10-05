using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawMapsService(Dictionary<long, RawMap> maps)
{
    public RawMap? GetMap(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMap> GetMaps() => maps.Values;
}
