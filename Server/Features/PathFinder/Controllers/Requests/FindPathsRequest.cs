using System.ComponentModel.DataAnnotations;

namespace DBI.Server.Features.PathFinder.Controllers.Requests;

/// <summary>
///     Search for a path between the start and end nodes.
/// </summary>
public class FindPathsRequest
{
    /// <summary>
    ///     The request for the start node.
    /// </summary>
    [Required]
    public required FindNodeRequest Start { get; init; }

    /// <summary>
    ///     The request for the end node.
    /// </summary>
    [Required]
    public required FindNodeRequest End { get; init; }
}
