namespace Server.Domains.DataCenter.Models.Extensions;

public static class MapPositionExtensions
{
    public static int DistanceTo(this RawMapPosition map, RawMapPosition otherMap) => Math.Abs(map.PosX - otherMap.PosX) + Math.Abs(map.PosX - otherMap.PosY);
}
