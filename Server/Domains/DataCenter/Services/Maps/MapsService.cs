using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.Maps;

public class MapsService(IReadOnlyCollection<MapPosition> maps)
{
    readonly Dictionary<long, MapPosition> _maps = maps.ToDictionary(map => map.MapId, map => map);

    public MapPosition? GetMap(long id) => _maps.GetValueOrDefault(id);
    public IEnumerable<MapPosition> GetMaps() => _maps.Values;
}

public static class MapsServiceExtensions
{
    public static IEnumerable<MapPosition> GetMapsAtPosition(this MapsService service, Position position) =>
        service.GetMaps().Where(map => map.PosX == position.X && map.PosY == position.Y);
}
