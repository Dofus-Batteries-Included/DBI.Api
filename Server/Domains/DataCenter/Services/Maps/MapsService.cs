using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.Maps;

public class MapsService(Dictionary<long, MapPositions> maps)
{
    public MapPositions? GetMap(long id) => maps.GetValueOrDefault(id);
    public IEnumerable<MapPositions> GetMaps() => maps.Values;
}

public static class MapsServiceExtensions
{
    public static IEnumerable<MapPositions> GetMapsAtPosition(this MapsService service, Position position) =>
        service.GetMaps().Where(map => map.PosX == position.X && map.PosY == position.Y);
}
