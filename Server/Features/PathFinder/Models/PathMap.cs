using Server.Common.Models;

namespace Server.Features.PathFinder.Models;

/// <summary>
///     A map in a path.
/// </summary>
public class PathMap
{
    /// <summary>
    ///     The unique ID of the map.
    /// </summary>
    public required long MapId { get; init; }

    /// <summary>
    ///     The coordinates of the map.
    /// </summary>
    public required Position? MapPosition { get; init; }

    /// <summary>
    ///     The unique ID of the node that corresponds to the current step of the path. <br />
    /// </summary>
    /// <remarks>
    ///     A node is a subset of cells in a map that are connected, so a map can contain multiple nodes.
    ///     This ID is the specific node that has been traversed by the path during the search.
    /// </remarks>
    public required long WorldGraphNodeId { get; init; }
}
