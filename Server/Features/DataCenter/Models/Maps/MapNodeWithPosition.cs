using DBI.Server.Common.Models;
using DBI.Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A node in a map with its position in the world.
/// </summary>
public class MapNodeWithPosition : MapNode
{
    /// <summary>
    ///     The coordinates of the corresponding map.
    /// </summary>
    public required Position? MapPosition { get; init; }
}

static class MapNodeWithPositionMappingExtensions
{
    public static MapNodeWithPosition Cook(this RawWorldGraphNode node, Position? position) =>
        new()
        {
            NodeId = node.Id,
            MapId = node.MapId,
            ZoneId = node.ZoneId,
            MapPosition = position
        };
}
