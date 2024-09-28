using System.Text.Json.Serialization;
using Server.Features.DataCenter.Models.WorldGraphs;

namespace Server.Features.PathFinder.Models;

/// <summary>
///     A step of a path.
/// </summary>
[JsonDerivedType(typeof(ScrollStep), "scroll")]
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
    public required WorldGraphEdgeDirection Direction { get; init; }
}
