using Server.Domains.DataCenter.Raw.Models;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawSubAreasService(Dictionary<long, RawSubArea> maps)
{
    public RawSubArea? GetSubArea(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawSubArea> GetSubAreas() => maps.Values;
}
