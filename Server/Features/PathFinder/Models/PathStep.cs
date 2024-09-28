using System.Text.Json.Serialization;
using Server.Common.Models;
using Server.Features.DataCenter.Models.WorldGraphs;

namespace Server.Features.PathFinder.Models;

[JsonDerivedType(typeof(ScrollStep), "scroll")]
public class PathStep
{
    public required long MapId { get; init; }
    public Position? MapPosition { get; init; }
}

public class ScrollStep : PathStep
{
    public required WorldGraphEdgeDirection Direction { get; init; }
}
