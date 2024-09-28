namespace Server.Features.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraph
{
    public IReadOnlyCollection<RawWorldGraphNode> Nodes { get; set; } = [];
    public IReadOnlyCollection<RawWorldGraphEdge> Edges { get; set; } = [];
}
