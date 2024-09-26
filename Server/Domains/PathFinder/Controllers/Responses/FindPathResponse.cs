using Server.Domains.PathFinder.Models;

namespace Server.Domains.PathFinder.Controllers.Responses;

public class FindPathResponse
{
    public bool FoundPath { get; set; }
    public IReadOnlyCollection<PathStep>? Steps { get; set; }
}
