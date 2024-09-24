using Server.Domains.DataCenter.Models.WorldGraphs;

namespace Server.Domains.DataCenter.Raw.Services.WorldGraphs;

public class WorldGraphService(WorldGraph data)
{
    readonly Dictionary<long, WorldGraphNode> _nodes = data.Nodes.ToDictionary(n => n.Id, n => n);
    readonly Dictionary<long, Dictionary<long, IReadOnlyCollection<WorldGraphEdge>>> _edges = data.Edges.GroupBy(e => e.From)
        .ToDictionary(g => g.Key, g => g.GroupBy(g => g.To).ToDictionary(gg => gg.Key, IReadOnlyCollection<WorldGraphEdge> (gg) => gg.ToArray()));

    public IEnumerable<WorldGraphNode> GetAllNodes() => _nodes.Values;
    public IEnumerable<WorldGraphEdge> GetAllEdges() => _edges.Values.SelectMany(d => d.SelectMany(dd => dd.Value));
    public WorldGraphNode? GetNode(long id) => _nodes.GetValueOrDefault(id);
    public IEnumerable<WorldGraphEdge> GetEdges(long from, long to) => _edges.GetValueOrDefault(from)?.GetValueOrDefault(to) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesFrom(long from) => _edges.GetValueOrDefault(from)?.SelectMany(d => d.Value) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesTo(long to) => _edges.Values.SelectMany(d => d.GetValueOrDefault(to) ?? []);
}
