﻿using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.DataCenter.Structured.Models.Maps;

/// <summary>
///     A transition between two nodes
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(MapTransitionMinimal), "unknown")]
[JsonDerivedType(typeof(MapScrollTransitionMinimal), "scroll")]
[JsonDerivedType(typeof(MapActionTransitionMinimal), "map-action")]
[JsonDerivedType(typeof(MapInteractiveTransitionMinimal), "interactive")]
[JsonDerivedType(typeof(MapNpcActionTransitionMinimal), "npc")]
public class MapTransitionMinimal
{
}

/// <summary>
///     A transition where the character needs to scroll in order to reach the next map.
/// </summary>
public class MapScrollTransitionMinimal : MapTransitionMinimal
{
    /// <summary>
    ///     The direction of the scroll between the start and end nodes.
    /// </summary>
    public Direction Direction { get; set; }
}

/// <summary>
///     A transition where the character needs to perform an action to reach the next map.
/// </summary>
public class MapActionTransitionMinimal : MapTransitionMinimal
{
    /// <summary>
    ///     The direction of the transition between the start and end nodes.
    /// </summary>
    public ExtendedDirection? Direction { get; set; }
}

/// <summary>
///     A transition where the character needs to interact with an interactive element in order to reach the next map.
/// </summary>
public class MapInteractiveTransitionMinimal : MapTransitionMinimal
{
    /// <summary>
    ///     The direction of the transition between the start and end nodes.
    /// </summary>
    public ExtendedDirection? Direction { get; set; }
}

/// <summary>
///     A transition where the character needs to interact with an NPC in order to reach the next map.
/// </summary>
public class MapNpcActionTransitionMinimal : MapTransitionMinimal
{
}

public static class MapTransitionMinimalMappingExtensions
{
    public static MapTransitionMinimal Cook(this RawWorldGraphEdgeTransition transition)
    {
        switch (transition.Type)
        {
            case RawWorldGraphEdgeType.Scroll:
            case RawWorldGraphEdgeType.ScrollAction:
                return new MapScrollTransitionMinimal
                {
                    Direction = transition.Direction?.Cook() ?? throw new InvalidOperationException("Could not find direction")
                };
            case RawWorldGraphEdgeType.MapAction:
                return new MapActionTransitionMinimal
                {
                    Direction = transition.Direction?.CookExtendedDirection()
                };
            case RawWorldGraphEdgeType.Interactive:
                return new MapInteractiveTransitionMinimal
                {
                    Direction = transition.Direction?.CookExtendedDirection()
                };
            case RawWorldGraphEdgeType.NpcAction:
                return new MapNpcActionTransitionMinimal();
            default:
                return new MapTransitionMinimal();
        }
    }
}
