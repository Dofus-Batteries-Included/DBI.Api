using Server.Features.DataCenter.Models.Maps;

namespace Server.Features.PathFinder.Models;

/// <summary>
///     A step of a path.
/// </summary>
public class PathStep
{
    /// <summary>
    ///     The node that is being traversed at this step.
    /// </summary>
    public required MapNodeWithPosition Node { get; init; }

    /// <summary>
    ///     The transition to take at this step to reach the next node.
    /// </summary>
    public required MapTransition? Transition { get; init; }
}
