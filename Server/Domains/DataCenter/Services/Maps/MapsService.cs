using Server.Common.Models;
using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.Maps;

public class MapsService(IReadOnlyCollection<RawMapPosition> maps)
{
    readonly Dictionary<long, RawMapPosition> _maps = maps.ToDictionary(map => map.MapId, map => map);

    public RawMapPosition? GetMap(long id) => _maps.GetValueOrDefault(id);
    public IEnumerable<RawMapPosition> GetMaps() => _maps.Values;
}

public static class MapsServiceExtensions
{
    public static IEnumerable<RawMapPosition> GetMapsAtPosition(this MapsService service, Position position) =>
        service.GetMaps().Where(map => map.PosX == position.X && map.PosY == position.Y);
}
