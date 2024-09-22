using Server.Common.Models;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Models.Extensions;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.WorldGraphs;
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
        RawMapPosition? sourceMap = _mapsService.GetPositionOfNode(sourceNode);
        RawMapPosition? targetMap = _mapsService.GetPositionOfNode(targetNode);
        Position? fromPosition = sourceMap == null ? null : new Position(sourceMap.PosX, sourceMap.PosY);
        Position? toPosition = targetMap == null ? null : new Position(targetMap.PosX, targetMap.PosY);

        if (sourceNode == targetNode)
        {
            return new Path
            {
                FromMapId = sourceNode.MapId,
                FromMapPosition = fromPosition,
                ToMapId = targetNode.MapId,
                ToMapPosition = toPosition,
                Steps = []
            };
        }

        if (!_knownPaths.ContainsKey((sourceNode.Id, targetNode.Id)))
        {
            _logger.LogDebug(
                "Cache miss while computing path from {SourceNodeId} ({SourceMapPosition}) to {TargetNodeId} ({TargetMapPosition})",
                sourceNode.Id,
                fromPosition,
                targetNode.Id,
                toPosition
            );
            ComputePath(sourceNode, targetNode);
        }

        return _knownPaths[(sourceNode.Id, targetNode.Id)];
    }

    void ComputePath(WorldGraphNode sourceNode, WorldGraphNode targetNode)
    {
        RawMapPosition? targetMap = _mapsService.GetPositionOfNode(targetNode);
        Position? toPosition = targetMap == null ? null : new Position(targetMap.PosX, targetMap.PosY);

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
            RawMapPosition? currentMap = _mapsService.GetPositionOfNode(current);
            Position? currentPosition = currentMap == null ? null : new Position(currentMap.PosX, currentMap.PosY);

            _knownPaths[(current.Id, targetNode.Id)] = new Path
            {
                FromMapId = current.MapId, FromMapPosition = currentPosition ?? new Position(),
                ToMapId = targetNode.MapId,
                ToMapPosition = toPosition ?? new Position(),
                Steps = Enumerable.Reverse(result).ToArray()
            };
        }
    }

    PathStep ComputeStep(WorldGraphNode previous, WorldGraphNode current)
    {
        WorldGraphEdge[] edges = _worldGraphService.GetEdges(previous.Id, current.Id).ToArray();
        WorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();

        WorldGraphEdgeTransition? scrollTransition = transitions.FirstOrDefault(t => t.Type is WorldGraphEdgeType.Scroll);

        if (scrollTransition is { Direction: not null })
        {
            return new ScrollStep { FromMapId = previous.MapId, ToMapId = current.MapId, Direction = scrollTransition.Direction.Value };
        }

        return new PathStep { FromMapId = previous.MapId, ToMapId = current.MapId };
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
        RawMapPosition? fromMap = _mapsService.GetMap(from.MapId);
        if (fromMap is null)
        {
            return 0;
        }

        RawMapPosition? toMap = _mapsService.GetMap(to.MapId);
        if (toMap == null)
        {
            return 0;
        }

        return fromMap.DistanceTo(toMap);
    }
}
