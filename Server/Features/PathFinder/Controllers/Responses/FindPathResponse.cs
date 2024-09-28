using Server.Common.Models;
using Server.Features.PathFinder.Models;

namespace Server.Features.PathFinder.Controllers.Responses;

public class FindPathResponse
{
    public bool FoundPath { get; set; }
    public required long FromMapId { get; init; }
    public required Position? FromMapPosition { get; init; }
    public required long ToMapId { get; init; }
    public required Position? ToMapPosition { get; init; }
    public IReadOnlyCollection<PathStep>? Steps { get; set; }
}
