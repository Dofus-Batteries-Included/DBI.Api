using System.Text.Json.Serialization;
using Server.Common.Models;
using Server.Domains.DataCenter.Models.WorldGraphs;

namespace Server.Domains.PathFinder.Models;

[JsonDerivedType(typeof(ScrollStep), "scroll")]
public class PathStep
{
    public required long FromMapId { get; init; }
    public Position? FromMapPosition { get; init; }
    public required long ToMapId { get; init; }
    public Position? ToMapPosition { get; init; }
}

public class ScrollStep : PathStep
{
    public required WorldGraphEdgeDirection Direction { get; init; }
}
