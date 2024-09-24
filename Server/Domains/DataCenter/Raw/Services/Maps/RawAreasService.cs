using Server.Domains.DataCenter.Raw.Models;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawAreasService(Dictionary<long, RawArea> maps)
{
    public RawArea? GetArea(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawArea> GetAreas() => maps.Values;
}
