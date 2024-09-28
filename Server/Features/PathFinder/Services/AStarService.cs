using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Models;
using Enumerable = System.Linq.Enumerable;
using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Services;

class AStarService
{
    const int MaxIterations = 100000;

    readonly RawWorldGraphService _rawWorldGraphService;
    readonly MapsService _mapsService;
    readonly ILogger _logger;

    readonly Dictionary<(long, long), Path?> _knownPaths = new();

    public AStarService(RawWorldGraphService rawWorldGraphService, MapsService mapsService, ILogger<AStarService> logger)
    {
        _rawWorldGraphService = rawWorldGraphService;
        _mapsService = mapsService;
        _logger = logger;
    }

    public Path? GetShortestPath(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode)
    {
        Map? sourceMap = _mapsService.GetMap(sourceNode);
        Map? targetMap = _mapsService.GetMap(targetNode);

        if (sourceNode == targetNode)
        {
            return new Path
            {
                From = new PathMap { MapId = sourceNode.MapId, MapPosition = sourceMap?.Position, WorldGraphNodeId = sourceNode.Id },
                To = new PathMap { MapId = targetNode.MapId, MapPosition = targetMap?.Position, WorldGraphNodeId = targetNode.Id },
                Steps = []
            };
        }

        if (!_knownPaths.ContainsKey((sourceNode.Id, targetNode.Id)))
        {
            _logger.LogDebug(
                "Cache miss while computing path from {SourceNodeId} ({SourceMapPosition}) to {TargetNodeId} ({TargetMapPosition})",
                sourceNode.Id,
                sourceMap?.Position,
                targetNode.Id,
                targetMap?.Position
            );
            ComputePath(sourceNode, targetNode);
        }

        return _knownPaths[(sourceNode.Id, targetNode.Id)];
    }

    void ComputePath(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode)
    {
        Map? targetMap = _mapsService.GetMap(targetNode);

        Dictionary<RawWorldGraphNode, RawWorldGraphNode> cameFrom = new();

        if (!Explore(sourceNode, targetNode, cameFrom))
        {
            _knownPaths.Add((sourceNode.Id, targetNode.Id), null);
            return;
        }

        List<PathStep> result = [];

        RawWorldGraphNode currentNode = targetNode;
        while (cameFrom.ContainsKey(currentNode))
        {
            RawWorldGraphNode previous = cameFrom[currentNode];

            PathStep step = ComputeStep(previous, currentNode);
            result.Add(step);

            currentNode = previous;
            Map? currentMap = _mapsService.GetMap(currentNode);

            _knownPaths[(currentNode.Id, targetNode.Id)] = new Path
            {
                From = new PathMap { MapId = currentNode.MapId, MapPosition = currentMap?.Position, WorldGraphNodeId = currentNode.Id },
                To = new PathMap { MapId = targetNode.MapId, MapPosition = targetMap?.Position, WorldGraphNodeId = targetNode.Id },
                Steps = Enumerable.Reverse(result).ToArray()
            };
        }
    }

    PathStep ComputeStep(RawWorldGraphNode current, RawWorldGraphNode next)
    {
        Map? currentMap = _mapsService.GetMap(current);
        PathMap currentPathMap = new() { MapId = current.MapId, MapPosition = currentMap?.Position, WorldGraphNodeId = current.Id };

        RawWorldGraphEdge[] edges = _rawWorldGraphService.GetEdges(current.Id, next.Id).ToArray();
        RawWorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();

        RawWorldGraphEdgeTransition? scrollTransition = transitions.FirstOrDefault(t => t.Type is RawWorldGraphEdgeType.Scroll or RawWorldGraphEdgeType.ScrollAction);

        if (scrollTransition is { Direction: not null })
        {
            return new ScrollStep
            {
                Map = currentPathMap,
                Direction = scrollTransition.Direction.Value
            };
        }

        return new PathStep { Map = currentPathMap };
    }

    bool Explore(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode, IDictionary<RawWorldGraphNode, RawWorldGraphNode> cameFrom)
    {
        HashSet<RawWorldGraphNode> closed = [];
        Dictionary<RawWorldGraphNode, int> open = [];
        Dictionary<RawWorldGraphNode, int> openCosts = [];

        open[sourceNode] = ComputeDistance(sourceNode, targetNode);
        openCosts[sourceNode] = 0;

        int iteration = 0;
        while (open.Count > 0 && iteration < MaxIterations)
        {
            RawWorldGraphNode currentNode = open.MinBy(kv => kv.Value).Key;
            open.Remove(currentNode);

            if (currentNode == targetNode)
            {
                return true;
            }

            int currentCost = openCosts[currentNode];

            foreach (long neighborId in GetNeighbors(currentNode))
            {
                RawWorldGraphNode? neighborNode = _rawWorldGraphService.GetNode(neighborId);
                if (neighborNode == null)
                {
                    continue;
                }

                if (closed.Contains(neighborNode) || openCosts.TryGetValue(neighborNode, out int neighborCost) && neighborCost < currentCost)
                {
                    continue;
                }

                openCosts[neighborNode] = currentCost + 1;
                open[neighborNode] = currentCost + ComputeDistance(neighborNode, targetNode);
                cameFrom[neighborNode] = currentNode;
            }

            closed.Add(currentNode);

            iteration++;
        }

        if (iteration > MaxIterations)
        {
            _logger.LogWarning("AStar ran out of juice");
        }

        return false;
    }

    IEnumerable<long> GetNeighbors(RawWorldGraphNode nodeId)
    {
        IEnumerable<RawWorldGraphEdge> edges = _rawWorldGraphService.GetEdgesFrom(nodeId.Id);
        return edges.Select(e => e.To);
    }

    int ComputeDistance(RawWorldGraphNode from, RawWorldGraphNode to)
    {
        Map? fromMap = _mapsService.GetMap(from.MapId);
        if (fromMap is null)
        {
            return 0;
        }

        Map? toMap = _mapsService.GetMap(to.MapId);
        if (toMap == null)
        {
            return 0;
        }

        return fromMap.Position.DistanceTo(toMap.Position);
    }
}
