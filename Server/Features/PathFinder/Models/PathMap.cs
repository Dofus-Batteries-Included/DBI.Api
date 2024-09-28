using Server.Common.Models;

namespace Server.Features.PathFinder.Models;

public class PathMap
{
    public required long MapId { get; init; }
    public required Position? MapPosition { get; init; }
    public required long WorldGraphNodeId { get; init; }
}
