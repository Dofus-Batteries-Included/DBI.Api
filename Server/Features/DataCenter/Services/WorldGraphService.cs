using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;

namespace Server.Features.DataCenter.Services;

/// <summary>
///     Get world graph data
/// </summary>
public class WorldGraphService(RawWorldGraphService? rawWorldGraphService)
{
    /// <summary>
    ///     Get all the nodes in the given map.
    /// </summary>
    public IEnumerable<MapNode>? GetNodesInMap(long mapId) => rawWorldGraphService?.GetNodesInMap(mapId).Select(Cook);

    static MapNode Cook(RawWorldGraphNode node) =>
        new()
        {
            Id = node.Id,
            MapId = node.MapId,
            ZoneId = node.ZoneId
        };
}
