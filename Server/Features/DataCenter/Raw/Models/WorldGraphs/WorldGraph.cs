namespace Server.Features.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class WorldGraph
{
    public IReadOnlyCollection<WorldGraphNode> Nodes { get; set; } = [];
    public IReadOnlyCollection<WorldGraphEdge> Edges { get; set; } = [];
}
