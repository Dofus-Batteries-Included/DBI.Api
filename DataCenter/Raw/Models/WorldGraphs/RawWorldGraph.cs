namespace DBI.DataCenter.Raw.Models.WorldGraphs;

/// <summary>
/// </summary>
public class RawWorldGraph
{
    public IReadOnlyCollection<RawWorldGraphNode> Nodes { get; set; } = [];
    public IReadOnlyCollection<RawWorldGraphEdge> Edges { get; set; } = [];
}
