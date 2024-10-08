using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawMapPositionsService(IReadOnlyCollection<RawMapPosition> maps)
{
    readonly Dictionary<long, RawMapPosition> _maps = maps.ToDictionary(map => map.MapId, map => map);

    public RawMapPosition? GetMap(long mapId) => _maps.GetValueOrDefault(mapId);
    public IEnumerable<RawMapPosition> GetMaps() => _maps.Values;
    public IEnumerable<RawMapPosition> GetMapsAtPosition(RawPosition position) => _maps.Values.Where(map => map.PosX == position.X && map.PosY == position.Y);
}
