using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

public class Map
{
    public required long? WorldMapId { get; init; }
    public required LocalizedText? WorldMapName { get; init; }
    public required long? SuperAreaId { get; init; }
    public required LocalizedText? SuperAreaName { get; init; }
    public required long? AreaId { get; init; }
    public required LocalizedText? AreaName { get; init; }
    public required long? SubAreaId { get; init; }
    public required LocalizedText? SubAreaName { get; init; }
    public required long MapId { get; init; }
    public required LocalizedText Name { get; init; }
    public required Position Position { get; init; }
    public required int CellsCount { get; init; }
}
