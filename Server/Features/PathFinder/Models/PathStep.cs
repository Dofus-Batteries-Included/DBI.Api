using System.Text.Json.Serialization;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Features.PathFinder.Models;

/// <summary>
///     A step of a path.
/// </summary>
[JsonDerivedType(typeof(ScrollStep), "scroll")]
[JsonDerivedType(typeof(InteractiveStep), "interactive")]
[JsonDerivedType(typeof(NpcActionStep), "npc")]
public class PathStep
{
    /// <summary>
    ///     The map that is being traversed at this step.
    /// </summary>
    public required PathMap Map { get; init; }
}

/// <summary>
///     A step where the character needs to scroll in order to reach the next map.
/// </summary>
public class ScrollStep : PathStep
{
    /// <summary>
    ///     The direction of the scroll to reach the next map.
    /// </summary>
    public required RawWorldGraphEdgeDirection Direction { get; init; }
}

/// <summary>
///     A step where the character needs to interact with an interactive element in order to reach the next map.
/// </summary>
public class InteractiveStep : PathStep
{
    /// <summary>
    ///     The unique ID of the interactive element to interact with.
    /// </summary>
    public required int InteractiveId { get; init; }
}

/// <summary>
///     A step where the character needs to interact with an NPC in order to reach the next map.
/// </summary>
public class NpcActionStep : PathStep
{
    /// <summary>
    ///     The unique ID of the NPC to interact with.
    /// </summary>
    public required int NpcId { get; init; }
}
