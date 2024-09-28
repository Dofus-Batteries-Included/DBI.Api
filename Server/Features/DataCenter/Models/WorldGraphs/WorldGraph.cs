namespace Server.Features.DataCenter.Models.WorldGraphs;

public class WorldGraph
{
    public IReadOnlyCollection<WorldGraphNode> Nodes { get; set; } = [];
    public IReadOnlyCollection<WorldGraphEdge> Edges { get; set; } = [];
}
