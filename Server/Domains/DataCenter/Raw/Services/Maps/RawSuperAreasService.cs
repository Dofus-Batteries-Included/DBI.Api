using Server.Domains.DataCenter.Raw.Models;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawSuperAreasService(Dictionary<long, RawSuperArea> maps)
{
    public RawSuperArea? GetSuperArea(long mapId) => maps.GetValueOrDefault(mapId);
    public IEnumerable<RawSuperArea> GetSuperAreas() => maps.Values;
}
