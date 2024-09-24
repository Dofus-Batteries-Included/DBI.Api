using Server.Domains.DataCenter.Models.WorldGraphs;

namespace Server.Domains.DataCenter.Raw.Services.WorldGraphs;

public class WorldGraphService(WorldGraph data)
{
    readonly Dictionary<long, WorldGraphNode> _nodes = data.Nodes.ToDictionary(n => n.Id, n => n);
    readonly Dictionary<long, Dictionary<long, IReadOnlyCollection<WorldGraphEdge>>> _edges = data.Edges.GroupBy(e => e.From)
        .ToDictionary(g => g.Key, g => g.GroupBy(g => g.To).ToDictionary(gg => gg.Key, IReadOnlyCollection<WorldGraphEdge> (gg) => gg.ToArray()));

    public IEnumerable<WorldGraphNode> GetAllNodes() => _nodes.Values;
    public IEnumerable<WorldGraphEdge> GetAllEdges() => _edges.Values.SelectMany(d => d.SelectMany(dd => dd.Value));
    public WorldGraphNode? GetNode(long nodeId) => _nodes.GetValueOrDefault(nodeId);
    public WorldGraphNode? GetNode(long mapId, int zoneId) => _nodes.Values.FirstOrDefault(n => n.MapId == mapId && n.ZoneId == zoneId);
    public IEnumerable<WorldGraphEdge> GetEdges(long fromNodeId, long toNodeId) => _edges.GetValueOrDefault(fromNodeId)?.GetValueOrDefault(toNodeId) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesFrom(long fromNodeId) => _edges.GetValueOrDefault(fromNodeId)?.SelectMany(d => d.Value) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesTo(long toNodeId) => _edges.Values.SelectMany(d => d.GetValueOrDefault(toNodeId) ?? []);
}
