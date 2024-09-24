using Server.Common.Models;
using Server.Domains.DataCenter.Models.Raw;

namespace Server.Domains.DataCenter.Services.Maps;

public class RawMapPositionsService(IReadOnlyCollection<RawMapPosition> maps)
{
    readonly Dictionary<long, RawMapPosition> _maps = maps.ToDictionary(map => map.MapId, map => map);

    public RawMapPosition? GetMap(long mapId) => _maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMapPosition> GetMaps() => _maps.Values;
}

public static class MapsServiceExtensions
{
    public static IEnumerable<RawMapPosition> GetMapsAtPosition(this RawMapPositionsService service, Position position) =>
        service.GetMaps().Where(map => map.PosX == position.X && map.PosY == position.Y);
}
