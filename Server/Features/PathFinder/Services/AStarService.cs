using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Models;
using Enumerable = System.Linq.Enumerable;
using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Services;

class AStarService
{
    const int MaxIterations = 100000;

    readonly WorldGraphService _worldGraphService;
    readonly MapsService _mapsService;
    readonly ILogger _logger;

    readonly Dictionary<(long, long), Path?> _knownPaths = new();

    public AStarService(WorldGraphService worldGraphService, MapsService mapsService, ILogger<AStarService> logger)
    {
        _worldGraphService = worldGraphService;
        _mapsService = mapsService;
        _logger = logger;
    }

    public Path? GetShortestPath(WorldGraphNode sourceNode, WorldGraphNode targetNode)
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

    void ComputePath(WorldGraphNode sourceNode, WorldGraphNode targetNode)
    {
        Map? targetMap = _mapsService.GetMap(targetNode);

        Dictionary<WorldGraphNode, WorldGraphNode> cameFrom = new();

        if (!Explore(sourceNode, targetNode, cameFrom))
        {
            _knownPaths.Add((sourceNode.Id, targetNode.Id), null);
            return;
        }

        List<PathStep> result = [];

        WorldGraphNode currentNode = targetNode;
        while (cameFrom.ContainsKey(currentNode))
        {
            WorldGraphNode previous = cameFrom[currentNode];

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

    PathStep ComputeStep(WorldGraphNode current, WorldGraphNode next)
    {
        Map? currentMap = _mapsService.GetMap(current);
        PathMap currentPathMap = new() { MapId = current.MapId, MapPosition = currentMap?.Position, WorldGraphNodeId = current.Id };

        WorldGraphEdge[] edges = _worldGraphService.GetEdges(current.Id, next.Id).ToArray();
        WorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();

        WorldGraphEdgeTransition? scrollTransition = transitions.FirstOrDefault(t => t.Type is WorldGraphEdgeType.Scroll or WorldGraphEdgeType.ScrollAction);

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

    bool Explore(WorldGraphNode sourceNode, WorldGraphNode targetNode, IDictionary<WorldGraphNode, WorldGraphNode> cameFrom)
    {
        HashSet<WorldGraphNode> closed = [];
        Dictionary<WorldGraphNode, int> open = [];
        Dictionary<WorldGraphNode, int> openCosts = [];

        open[sourceNode] = ComputeDistance(sourceNode, targetNode);
        openCosts[sourceNode] = 0;

        int iteration = 0;
        while (open.Count > 0 && iteration < MaxIterations)
        {
            WorldGraphNode currentNode = open.MinBy(kv => kv.Value).Key;
            open.Remove(currentNode);

            if (currentNode == targetNode)
            {
                return true;
            }

            int currentCost = openCosts[currentNode];

            foreach (long neighborId in GetNeighbors(currentNode))
            {
                WorldGraphNode? neighborNode = _worldGraphService.GetNode(neighborId);
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

    IEnumerable<long> GetNeighbors(WorldGraphNode nodeId)
    {
        IEnumerable<WorldGraphEdge> edges = _worldGraphService.GetEdgesFrom(nodeId.Id);
        return edges.Select(e => e.To);
    }

    int ComputeDistance(WorldGraphNode from, WorldGraphNode to)
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
