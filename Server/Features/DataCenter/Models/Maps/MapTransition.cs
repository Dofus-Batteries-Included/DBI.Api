using System.Text.Json.Serialization;
using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A transition between two nodes
/// </summary>
[JsonDerivedType(typeof(MapScrollTransition), "scroll")]
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
///     A transition where the character needs to interact with an interactive element in order to reach the next map.
/// </summary>
public class MapInteractiveTransition : MapTransition
{
    /// <summary>
    ///     The unique ID of the interactive element to interact with.
    /// </summary>
    public int InteractiveElementId { get; set; }
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
