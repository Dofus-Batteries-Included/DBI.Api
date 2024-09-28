using Server.Features.DataCenter.Raw.Models;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawMapsService(Dictionary<long, RawMap> maps)
{
    public RawMap? GetMap(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMap> GetMaps() => maps.Values;
}
