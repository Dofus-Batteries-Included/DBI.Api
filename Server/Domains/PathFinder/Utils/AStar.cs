using Microsoft.Extensions.Logging.Abstractions;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Models.Extensions;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.WorldGraphs;
using Server.Domains.PathFinder.Models;
using Enumerable = System.Linq.Enumerable;
using Path = Server.Domains.PathFinder.Models.Path;

namespace Server.Domains.PathFinder.Utils;

class AStar
{
    const int MaxIterations = 100000;
    static readonly ILogger Log = NullLogger.Instance;

    readonly WorldGraphService _worldGraphService;
    readonly MapsService _mapsService;
    readonly Dictionary<(long, long), Path?> _knownPaths = new();

    public AStar(WorldGraphService worldGraphService, MapsService mapsService)
    {
        _worldGraphService = worldGraphService;
        _mapsService = mapsService;
    }

    public Path? GetShortestPath(long sourceNodeId, long targetNodeId)
    {
        if (sourceNodeId == targetNodeId)
        {
            return Path.Empty(sourceNodeId);
        }

        if (!_knownPaths.ContainsKey((sourceNodeId, targetNodeId)))
        {
            ComputePath(sourceNodeId, targetNodeId);
        }

        return _knownPaths[(sourceNodeId, targetNodeId)];
    }

    void ComputePath(long sourceNodeId, long targetNodeId)
    {
        WorldGraphNode? sourceNode = _worldGraphService.GetNode(sourceNodeId);
        if (sourceNode == null)
        {
            _knownPaths.Add((sourceNodeId, targetNodeId), null);
            return;
        }

        WorldGraphNode? targetNode = _worldGraphService.GetNode(targetNodeId);
        if (targetNode == null)
        {
            _knownPaths.Add((sourceNodeId, targetNodeId), null);
            return;
        }

        Log.LogDebug("Cache miss while computing path from {SourceMap} to {TargetMap}", sourceNodeId, targetNodeId);

        Dictionary<long, long> cameFrom = new();

        if (!Explore(sourceNode, targetNode, cameFrom))
        {
            _knownPaths.Add((sourceNodeId, targetNodeId), null);
            return;
        }

        List<PathStep> result = [];

        long current = targetNodeId;
        while (cameFrom.ContainsKey(current))
        {
            long previous = cameFrom[current];

            PathStep step = ComputeStep(previous, current);
            result.Add(step);

            current = previous;

            _knownPaths[(current, targetNodeId)] = new Path { FromMapId = sourceNodeId, ToMapId = targetNodeId, Steps = Enumerable.Reverse(result).ToArray() };
        }
    }

    PathStep ComputeStep(long previous, long current)
    {
        WorldGraphEdge[] edges = _worldGraphService.GetEdges(previous, current).ToArray();
        WorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();

        WorldGraphEdgeTransition? scrollTransition = transitions.FirstOrDefault(t => t.Type is WorldGraphEdgeType.Scroll);

        if (scrollTransition is { Direction: not null })
        {
            return new ScrollStep { FromMapId = previous, ToMapId = current, Direction = scrollTransition.Direction.Value };
        }

        return new PathStep { FromMapId = previous, ToMapId = current };
    }

    bool Explore(WorldGraphNode sourceNode, WorldGraphNode targetNode, IDictionary<long, long> cameFrom)
    {
        HashSet<long> closed = [];
        Dictionary<long, int> open = [];
        Dictionary<long, int> openCosts = [];

        open[sourceNode.Id] = ComputeDistance(sourceNode, targetNode);
        openCosts[sourceNode.Id] = 0;

        int iteration = 0;
        while (open.Count > 0 && iteration < MaxIterations)
        {
            long currentMapId = open.MinBy(kv => kv.Value).Key;
            open.Remove(currentMapId);

            if (currentMapId == targetNode.Id)
            {
                return true;
            }

            int currentCost = openCosts[currentMapId];

            foreach (long neighborId in GetNeighbors(currentMapId))
            {
                WorldGraphNode? neighborNode = _worldGraphService.GetNode(neighborId);
                if (neighborNode == null)
                {
                    continue;
                }

                if (closed.Contains(neighborId) || openCosts.TryGetValue(neighborId, out int neighborCost) && neighborCost < currentCost)
                {
                    continue;
                }

                openCosts[neighborId] = currentCost + 1;
                open[neighborId] = currentCost + ComputeDistance(neighborNode, targetNode);
                cameFrom[neighborId] = currentMapId;
            }

            closed.Add(currentMapId);

            iteration++;
        }

        if (iteration > MaxIterations)
        {
            Log.LogWarning("AStar ran out of juice");
        }

        return false;
    }

    IEnumerable<long> GetNeighbors(long nodeId)
    {
        IEnumerable<WorldGraphEdge> edges = _worldGraphService.GetEdgesFrom(nodeId);
        return edges.Select(e => e.To);
    }

    int ComputeDistance(WorldGraphNode from, WorldGraphNode to)
    {
        MapPositions? fromMap = _mapsService.GetMap(from.MapId);
        if (fromMap is null)
        {
            return 0;
        }

        MapPositions? toMap = _mapsService.GetMap(to.MapId);
        if (toMap == null)
        {
            return 0;
        }

        return fromMap.DistanceTo(toMap);
    }
}
