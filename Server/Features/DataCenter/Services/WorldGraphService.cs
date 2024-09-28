using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Features.DataCenter.Services;

/// <summary>
///     Get world graph data
/// </summary>
public class WorldGraphService(Raw.Services.WorldGraphs.WorldGraphService? rawWorldGraphService)
{
    /// <summary>
    ///     Get all the nodes in the given map.
    /// </summary>
    public IEnumerable<MapNode>? GetNodesInMap(long mapId) => rawWorldGraphService?.GetNodesInMap(mapId).Select(Cook);

    static MapNode Cook(WorldGraphNode node) =>
        new()
        {
            Id = node.Id,
            MapId = node.MapId,
            ZoneId = node.ZoneId
        };
}
