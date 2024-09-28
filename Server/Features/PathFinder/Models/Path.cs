namespace Server.Features.PathFinder.Models;

public class Path
{
    public required PathMap From { get; init; }
    public required PathMap To { get; init; }
    public required IReadOnlyCollection<PathStep> Steps { get; init; }
}
