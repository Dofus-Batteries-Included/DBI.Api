using Server.Common.Models;
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

    /// <summary>
    ///     Get all the transitions going out of the given map.
    /// </summary>
    public IEnumerable<MapTransition>? GetTransitionsFromMap(long mapId) =>
        rawWorldGraphService?.GetNodesInMap(mapId)
            .SelectMany(
                n => rawWorldGraphService.GetEdgesFrom(n.Id)
                    .SelectMany(
                        e =>
                        {
                            RawWorldGraphNode? fromNode = rawWorldGraphService.GetNode(e.From);
                            RawWorldGraphNode? toNode = rawWorldGraphService.GetNode(e.To);

                            return fromNode != null && toNode != null && e.Transitions != null ? e.Transitions.Select(t => Cook(fromNode, toNode, t)) : [];
                        }
                    )
            );

    /// <summary>
    ///     Get all the transitions going in the given map.
    /// </summary>
    public IEnumerable<MapTransition>? GetTransitionsToMap(long mapId) =>
        rawWorldGraphService?.GetNodesInMap(mapId)
            .SelectMany(
                n => rawWorldGraphService.GetEdgesTo(n.Id)
                    .SelectMany(
                        e =>
                        {
                            RawWorldGraphNode? fromNode = rawWorldGraphService.GetNode(e.From);
                            RawWorldGraphNode? toNode = rawWorldGraphService.GetNode(e.To);

                            return fromNode != null && toNode != null && e.Transitions != null ? e.Transitions.Select(t => Cook(fromNode, toNode, t)) : [];
                        }
                    )
            );

    static MapNode Cook(RawWorldGraphNode node) =>
        new()
        {
            Id = node.Id,
            MapId = node.MapId,
            ZoneId = node.ZoneId
        };

    static MapTransition Cook(RawWorldGraphNode from, RawWorldGraphNode to, RawWorldGraphEdgeTransition transition)
    {
        switch (transition.Type)
        {
            case RawWorldGraphEdgeType.Scroll:
            case RawWorldGraphEdgeType.ScrollAction:
                return new MapScrollTransition
                {
                    From = Cook(from),
                    To = Cook(to),
                    Direction = Cook(transition.Direction)
                };
            case RawWorldGraphEdgeType.Interactive:
                return new MapInteractiveTransition
                {
                    From = Cook(from),
                    To = Cook(to),
                    InteractiveElementId = transition.SkillId
                };
            case RawWorldGraphEdgeType.NpcAction:
                return new MapNpcActionTransition
                {
                    From = Cook(from),
                    To = Cook(to),
                    NpcId = transition.SkillId
                };
            default:
                return new MapTransition
                {
                    From = Cook(from),
                    To = Cook(to)
                };
        }
    }

    static Direction Cook(RawWorldGraphEdgeDirection? direction)
    {
        switch (direction)
        {
            case RawWorldGraphEdgeDirection.North:
                return Direction.North;
            case RawWorldGraphEdgeDirection.East:
                return Direction.East;
            case RawWorldGraphEdgeDirection.South:
                return Direction.South;
            case RawWorldGraphEdgeDirection.West:
                return Direction.West;
            case null:
            case RawWorldGraphEdgeDirection.Random:
            case RawWorldGraphEdgeDirection.Same:
            case RawWorldGraphEdgeDirection.Opposite:
            case RawWorldGraphEdgeDirection.Invalid:
            case RawWorldGraphEdgeDirection.SouthEast:
            case RawWorldGraphEdgeDirection.SouthWest:
            case RawWorldGraphEdgeDirection.NorthWest:
            case RawWorldGraphEdgeDirection.NorthEast:
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
