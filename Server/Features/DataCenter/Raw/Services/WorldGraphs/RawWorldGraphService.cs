using DBI.Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.Server.Features.DataCenter.Raw.Services.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraphService(RawWorldGraph data)
{
    readonly Dictionary<long, RawWorldGraphNode> _nodes = data.Nodes.ToDictionary(n => n.Id, n => n);
    readonly Dictionary<long, Dictionary<long, IReadOnlyCollection<RawWorldGraphEdge>>> _edges = data.Edges.GroupBy(e => e.From)
        .ToDictionary(g => g.Key, g => g.GroupBy(g => g.To).ToDictionary(gg => gg.Key, IReadOnlyCollection<RawWorldGraphEdge> (gg) => gg.ToArray()));

    public IEnumerable<RawWorldGraphNode> GetAllNodes() => _nodes.Values;
    public IEnumerable<RawWorldGraphEdge> GetAllEdges() => _edges.Values.SelectMany(d => d.SelectMany(dd => dd.Value));
    public RawWorldGraphNode? GetNode(long nodeId) => _nodes.GetValueOrDefault(nodeId);
    public RawWorldGraphNode? GetNode(long mapId, int zoneId) => _nodes.Values.FirstOrDefault(n => n.MapId == mapId && n.ZoneId == zoneId);
    public IEnumerable<RawWorldGraphNode> GetNodesInMap(long mapId) => _nodes.Values.Where(n => n.MapId == mapId);
    public IEnumerable<RawWorldGraphEdge> GetEdges(long fromNodeId, long toNodeId) => _edges.GetValueOrDefault(fromNodeId)?.GetValueOrDefault(toNodeId) ?? [];
    public IEnumerable<RawWorldGraphEdge> GetEdgesFrom(long fromNodeId) => _edges.GetValueOrDefault(fromNodeId)?.SelectMany(d => d.Value) ?? [];
    public IEnumerable<RawWorldGraphEdge> GetEdgesTo(long toNodeId) => _edges.Values.SelectMany(d => d.GetValueOrDefault(toNodeId) ?? []);
}
