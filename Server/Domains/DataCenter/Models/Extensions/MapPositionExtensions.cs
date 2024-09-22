namespace Server.Domains.DataCenter.Models.Extensions;

public static class MapPositionExtensions
{
    public static int DistanceTo(this MapPosition map, MapPosition otherMap) => Math.Abs(map.PosX - otherMap.PosX) + Math.Abs(map.PosX - otherMap.PosY);
}
