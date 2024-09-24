using Server.Domains.DataCenter.Raw.Models;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawMapsService(Dictionary<long, RawMap> maps)
{
    public RawMap? GetMap(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMap> GetMaps() => maps.Values;
}
