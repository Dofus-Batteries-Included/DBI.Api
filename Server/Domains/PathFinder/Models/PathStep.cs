using Server.Domains.DataCenter.Models.WorldGraphs;

namespace Server.Domains.PathFinder.Models;

public class PathStep
{
    public required long FromMapId { get; init; }
    public required long ToMapId { get; init; }
}

public class ScrollStep : PathStep
{
    public required WorldGraphEdgeDirection Direction { get; init; }
}
