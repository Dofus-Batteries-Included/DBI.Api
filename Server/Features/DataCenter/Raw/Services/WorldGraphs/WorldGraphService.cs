﻿using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Features.DataCenter.Raw.Services.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class WorldGraphService(WorldGraph data)
{
    readonly Dictionary<long, WorldGraphNode> _nodes = data.Nodes.ToDictionary(n => n.Id, n => n);
    readonly Dictionary<long, Dictionary<long, IReadOnlyCollection<WorldGraphEdge>>> _edges = data.Edges.GroupBy(e => e.From)
        .ToDictionary(g => g.Key, g => g.GroupBy(g => g.To).ToDictionary(gg => gg.Key, IReadOnlyCollection<WorldGraphEdge> (gg) => gg.ToArray()));

    public IEnumerable<WorldGraphNode> GetAllNodes() => _nodes.Values;
    public IEnumerable<WorldGraphEdge> GetAllEdges() => _edges.Values.SelectMany(d => d.SelectMany(dd => dd.Value));
    public WorldGraphNode? GetNode(long nodeId) => _nodes.GetValueOrDefault(nodeId);
    public WorldGraphNode? GetNode(long mapId, int zoneId) => _nodes.Values.FirstOrDefault(n => n.MapId == mapId && n.ZoneId == zoneId);
    public IEnumerable<WorldGraphNode> GetNodesInMap(long mapId) => _nodes.Values.Where(n => n.MapId == mapId);
    public IEnumerable<WorldGraphEdge> GetEdges(long fromNodeId, long toNodeId) => _edges.GetValueOrDefault(fromNodeId)?.GetValueOrDefault(toNodeId) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesFrom(long fromNodeId) => _edges.GetValueOrDefault(fromNodeId)?.SelectMany(d => d.Value) ?? [];
    public IEnumerable<WorldGraphEdge> GetEdgesTo(long toNodeId) => _edges.Values.SelectMany(d => d.GetValueOrDefault(toNodeId) ?? []);
}
