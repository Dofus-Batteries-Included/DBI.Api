using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.DataCenter.Structured.Models.Maps;

/// <summary>
///     A node in a map with its position in the world.
/// </summary>
public class MapNodeWithPosition : MapNode
{
    /// <summary>
    ///     The coordinates of the corresponding map.
    /// </summary>
    public required RawPosition? MapPosition { get; init; }
}

public static class MapNodeWithPositionMappingExtensions
{
    public static MapNodeWithPosition Cook(this RawWorldGraphNode node, RawPosition? position) =>
        new()
        {
            NodeId = node.Id,
            MapId = node.MapId,
            ZoneId = node.ZoneId,
            MapPosition = position
        };
}
