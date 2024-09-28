using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Controllers.Responses;

public class FindPathResponse
{
    public bool FoundPath { get; set; }
    public Path? Path { get; set; }
}
