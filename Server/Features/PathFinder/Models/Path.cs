using Server.Common.Models;

namespace Server.Features.PathFinder.Models;

public class Path
{
    public required long FromMapId { get; init; }
    public required Position? FromMapPosition { get; init; }
    public required long ToMapId { get; init; }
    public required Position? ToMapPosition { get; init; }
    public required IReadOnlyCollection<PathStep> Steps { get; init; }
}
