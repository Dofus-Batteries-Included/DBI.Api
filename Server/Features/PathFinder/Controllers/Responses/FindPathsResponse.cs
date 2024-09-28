using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Controllers.Responses;

public class FindPathsResponse
{
    public IReadOnlyCollection<Path>? Paths { get; set; }
}
