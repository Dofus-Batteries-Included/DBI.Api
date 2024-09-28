using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

public class WorldMap
{
    public required int WorldMapId { get; init; }
    public required LocalizedText? Name { get; init; }
    public required Position Origin { get; init; }
}
