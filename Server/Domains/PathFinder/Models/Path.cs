namespace Server.Domains.PathFinder.Models;

public class Path
{
    public required long FromMapId { get; init; }
    public required long ToMapId { get; init; }
    public required IReadOnlyCollection<PathStep> Steps { get; init; }

    public static Path Empty(long mapId) => new() { FromMapId = mapId, ToMapId = mapId, Steps = [] };
}
