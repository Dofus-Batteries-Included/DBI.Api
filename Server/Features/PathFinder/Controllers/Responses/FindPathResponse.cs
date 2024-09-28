using Server.Features.PathFinder.Models;

namespace Server.Features.PathFinder.Controllers.Responses;

public class FindPathResponse
{
    public bool FoundPath { get; set; }
    public IReadOnlyCollection<PathStep>? Steps { get; set; }
}
