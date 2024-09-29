using System.Text.Json.Serialization;
using Server.Common.Models;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A transition between two nodes
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MapTransition), "unknown")]
[JsonDerivedType(typeof(MapScrollTransition), "scroll")]
[JsonDerivedType(typeof(MapActionTransition), "map-action")]
[JsonDerivedType(typeof(MapInteractiveTransition), "interactive")]
[JsonDerivedType(typeof(MapNpcActionTransition), "npc")]
public class MapTransition
{
    /// <summary>
    ///     The start node.
    /// </summary>
    public required MapNode From { get; set; }

    /// <summary>
    ///     The end node.
    /// </summary>
    public required MapNode To { get; set; }
}

/// <summary>
///     A transition where the character needs to scroll in order to reach the next map.
/// </summary>
public class MapScrollTransition : MapTransition
{
    /// <summary>
    ///     The direction of the scroll between the start and end nodes.
    /// </summary>
    public Direction Direction { get; set; }
}

/// <summary>
///     A transition where the character needs to perform an action to reach the next map.
/// </summary>
public class MapActionTransition : MapTransition
{
}

/// <summary>
///     A transition where the character needs to interact with an interactive element in order to reach the next map.
/// </summary>
public class MapInteractiveTransition : MapTransition
{
}

/// <summary>
///     A transition where the character needs to interact with an NPC in order to reach the next map.
/// </summary>
public class MapNpcActionTransition : MapTransition
{
    /// <summary>
    ///     The unique ID of the NPC to interact with.
    /// </summary>
    public int NpcId { get; set; }
}

static class MapTransitionMappingExtensions
{
    public static MapTransition Cook(this RawWorldGraphEdgeTransition transition, RawWorldGraphNode from, RawWorldGraphNode to)
    {
        switch (transition.Type)
        {
            case RawWorldGraphEdgeType.Scroll:
            case RawWorldGraphEdgeType.ScrollAction:
                return new MapScrollTransition
                {
                    From = from.Cook(),
                    To = to.Cook(),
                    Direction = transition.Direction.Cook()
                };
            case RawWorldGraphEdgeType.MapAction:
                return new MapActionTransition
                {
                    From = from.Cook(),
                    To = to.Cook()
                };
            case RawWorldGraphEdgeType.Interactive:
                return new MapInteractiveTransition
                {
                    From = from.Cook(),
                    To = to.Cook()
                };
            case RawWorldGraphEdgeType.NpcAction:
                return new MapNpcActionTransition
                {
                    From = from.Cook(),
                    To = to.Cook(),
                    NpcId = transition.SkillId
                };
            default:
                return new MapTransition
                {
                    From = from.Cook(),
                    To = to.Cook()
                };
        }
    }
}
