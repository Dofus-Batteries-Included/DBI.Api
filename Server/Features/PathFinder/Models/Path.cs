using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.Server.Features.PathFinder.Models;

/// <summary>
///     A path between two maps.
/// </summary>
/// <remarks>
///     More specifically: this is a path between two nodes of the world graph.
///     A node is a subset of cells in a map that are connected, so a map can contain multiple nodes.
///     See <see cref="From" /> and <see cref="To" /> to get the IDs of the actual nodes of this path.
/// </remarks>
public class Path
{
    /// <summary>
    ///     The start map.
    /// </summary>
    public required MapNodeWithPosition From { get; init; }

    /// <summary>
    ///     The end map.
    /// </summary>
    public required MapNodeWithPosition To { get; init; }

    /// <summary>
    ///     The steps from <see cref="From" /> that a character must take to reach <see cref="To" />.
    /// </summary>
    public required IReadOnlyCollection<PathStep> Steps { get; init; }
}
