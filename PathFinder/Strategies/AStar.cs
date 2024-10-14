using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Extensions;
using Microsoft.Extensions.Logging;

namespace DBI.PathFinder.Strategies;

public class AStar : IPathFindingStrategy
{
    const int MaxIterations = 100000;

    readonly IWorldDataProvider _worldDataProvider;
    readonly ILogger _logger;
    readonly Dictionary<(long, long), IReadOnlyList<RawWorldGraphNode>?> _knownPaths = new();

    public AStar(IWorldDataProvider worldDataProvider, ILogger logger)
    {
        _worldDataProvider = worldDataProvider;
        _logger = logger;
    }

    public IReadOnlyList<RawWorldGraphNode>? ComputePath(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode)
    {
        if (!_knownPaths.ContainsKey((sourceNode.Id, targetNode.Id)))
        {
            Dictionary<RawWorldGraphNode, RawWorldGraphNode> cameFrom = new();

            if (!Explore(sourceNode, targetNode, cameFrom))
            {
                _knownPaths.Add((sourceNode.Id, targetNode.Id), null);
                return null;
            }

            List<RawWorldGraphNode> result = [targetNode];
            RawWorldGraphNode currentNode = targetNode;
            while (cameFrom.ContainsKey(currentNode))
            {
                RawWorldGraphNode previous = cameFrom[currentNode];
                result.Add(previous);

                currentNode = previous;

                _knownPaths[(currentNode.Id, targetNode.Id)] = Enumerable.Reverse(result).ToArray();
            }

            return _knownPaths[(sourceNode.Id, targetNode.Id)];
        }

        return _knownPaths.GetValueOrDefault((sourceNode.Id, targetNode.Id));
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
                RawWorldGraphNode? neighborNode = _worldDataProvider.GetNode(neighborId);
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
        IEnumerable<RawWorldGraphEdge> edges = _worldDataProvider.GetEdgesFromNode(nodeId.Id);
        return edges.Select(e => e.To);
    }

    int ComputeDistance(RawWorldGraphNode from, RawWorldGraphNode to)
    {
        Map? fromMap = _worldDataProvider.GetMap(from.MapId);
        if (fromMap is null)
        {
            return 0;
        }

        Map? toMap = _worldDataProvider.GetMap(to.MapId);
        if (toMap == null)
        {
            return 0;
        }

        return fromMap.Position.DistanceTo(toMap.Position);
    }
}
