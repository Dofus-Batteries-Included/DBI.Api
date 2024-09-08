using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.Maps;

public interface IMapsService
{
    event EventHandler DataRefreshed;

    Map? GetMap(long id);
    IEnumerable<Map> GetMaps();
}

public static class MapsServiceExtensions
{
    public static IEnumerable<Map> GetMapsAtPosition(this IMapsService service, Position position) =>
        service.GetMaps().Where(map => map.PosX == position.X && map.PosY == position.Y);
}
