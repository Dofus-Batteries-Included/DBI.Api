using Server.Domains.DataCenter.Models.Raw;

namespace Server.Domains.DataCenter.Services.Maps;

public class RawMapsService(Dictionary<long, RawMap> maps)
{
    public RawMap? GetMap(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMap> GetMaps() => maps.Values;
}
