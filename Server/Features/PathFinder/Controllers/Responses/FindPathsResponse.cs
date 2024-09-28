using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Controllers.Responses;

/// <summary>
///     Paths that have been found.
/// </summary>
public class FindPathsResponse
{
    /// <summary>
    ///     All the paths that have been found.
    /// </summary>
    public IReadOnlyCollection<Path> Paths { get; set; } = [];
}
