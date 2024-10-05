using Path = DBI.Server.Features.PathFinder.Models.Path;

namespace DBI.Server.Features.PathFinder.Controllers.Responses;

/// <summary>
///     Path that have been found (or not found).
/// </summary>
public class FindPathResponse
{
    /// <summary>
    ///     Has the path been found?
    /// </summary>
    public bool PathFound { get; set; }

    /// <summary>
    ///     The path that has been found, if any.
    /// </summary>
    public Path? Path { get; set; }
}
