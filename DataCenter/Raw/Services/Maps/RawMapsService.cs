using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawMapsService(Dictionary<long, RawMap> maps)
{
    public RawMap? GetMap(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMap> GetMaps() => maps.Values;
}
