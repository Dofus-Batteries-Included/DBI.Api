using Server.Common.Models;
using Server.Domains.DataCenter.Models.Maps;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Raw.Services.WorldGraphs;
using Server.Domains.DataCenter.Services;
using Server.Domains.PathFinder.Models;
using Enumerable = System.Linq.Enumerable;
using Path = Server.Domains.PathFinder.Models.Path;

namespace Server.Domains.PathFinder.Services;

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
                FromMapId = sourceNode.MapId,
                FromMapPosition = sourceMap?.Position,
                ToMapId = targetNode.MapId,
                ToMapPosition = targetMap?.Position,
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

        WorldGraphNode current = targetNode;
        while (cameFrom.ContainsKey(current))
        {
            WorldGraphNode previous = cameFrom[current];

            PathStep step = ComputeStep(previous, current);
            result.Add(step);

            current = previous;
            Map? currentMap = _mapsService.GetMap(current);

            _knownPaths[(current.Id, targetNode.Id)] = new Path
            {
                FromMapId = current.MapId,
                FromMapPosition = currentMap?.Position ?? new Position(),
                ToMapId = targetNode.MapId,
                ToMapPosition = targetMap?.Position ?? new Position(),
                Steps = Enumerable.Reverse(result).ToArray()
            };
        }
    }

    PathStep ComputeStep(WorldGraphNode previous, WorldGraphNode current)
    {
        Map? previousMap = _mapsService.GetMap(previous);
        Map? currentMap = _mapsService.GetMap(current);

        WorldGraphEdge[] edges = _worldGraphService.GetEdges(previous.Id, current.Id).ToArray();
        WorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();

        WorldGraphEdgeTransition? scrollTransition = transitions.FirstOrDefault(t => t.Type is WorldGraphEdgeType.Scroll or WorldGraphEdgeType.ScrollAction);

        if (scrollTransition is { Direction: not null })
        {
            return new ScrollStep
            {
                MapId = previous.MapId,
                MapPosition = previousMap?.Position,
                Direction = scrollTransition.Direction.Value
            };
        }

        return new PathStep { MapId = previous.MapId, MapPosition = previousMap?.Position };
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
