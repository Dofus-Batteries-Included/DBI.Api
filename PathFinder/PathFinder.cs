using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Models;
using DBI.PathFinder.Strategies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Path = DBI.PathFinder.Models.Path;

namespace DBI.PathFinder;

public class PathFinder
{
    readonly IPathFindingStrategy _pathFindingStrategy;
    readonly IWorldDataProvider _worldDataProvider;

    public PathFinder(IWorldDataProvider worldDataProvider, ILogger? logger = null) : this(new AStar(worldDataProvider, logger ?? NullLogger.Instance), worldDataProvider) { }

    public PathFinder(IPathFindingStrategy pathFindingStrategy, IWorldDataProvider worldDataProvider)
    {
        _pathFindingStrategy = pathFindingStrategy;
        _worldDataProvider = worldDataProvider;
    }

    public Path? GetShortestPath(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode)
    {
        IReadOnlyList<RawWorldGraphNode>? rawPath = _pathFindingStrategy.ComputePath(sourceNode, targetNode);
        if (rawPath == null)
        {
            return null;
        }

        Map? sourceMap = _worldDataProvider.GetMapOfNode(sourceNode);
        Map? targetMap = _worldDataProvider.GetMapOfNode(targetNode);

        List<PathStep> steps = new(rawPath.Count);
        for (int i = 0; i < rawPath.Count - 1; i++)
        {
            RawWorldGraphNode current = rawPath[i];
            RawWorldGraphNode next = rawPath[i + 1];
            PathStep step = ComputeStep(current, next);
            steps.Add(step);
        }

        return new Path
        {
            From = sourceNode.Cook(sourceMap?.Position),
            To = targetNode.Cook(targetMap?.Position),
            Steps = steps
        };
    }

    public IEnumerable<MapNodeWithPosition> EnumerateNodesInDirection(RawWorldGraphNode sourceNode, Direction direction)
    {
        RawWorldGraphNode current = sourceNode;
        while (true)
        {
            IEnumerable<RawWorldGraphEdge> edges = _worldDataProvider.GetEdgesFromNode(current.Id);
            var transitionInDirection = edges.Select(
                    e => new { Edge = e, Transitions = e.Transitions?.Where(t => t.Direction is not null && DirectionEquals(t.Direction.Value, direction)).ToArray() ?? [] }
                )
                .Where(x => x.Transitions.Length > 0)
                .ToArray();

            if (transitionInDirection.Length == 0)
            {
                yield break;
            }

            // Sometimes, there are mutliple transitions in the same direction, e.g. map 196345861 zone 2 has both a scroll and a map-action transition north 
            // In that case, we choose scroll actions first, then scroll actions, then map actions, then interactives, then whatever transition is available

            var edgeAndTransition = transitionInDirection.FirstOrDefault(x => x.Transitions.Any(t => t.Type == RawWorldGraphEdgeType.Scroll))
                                    ?? transitionInDirection.FirstOrDefault(x => x.Transitions.Any(t => t.Type == RawWorldGraphEdgeType.ScrollAction))
                                    ?? transitionInDirection.FirstOrDefault(x => x.Transitions.Any(t => t.Type == RawWorldGraphEdgeType.MapAction))
                                    ?? transitionInDirection.FirstOrDefault(x => x.Transitions.Any(t => t.Type == RawWorldGraphEdgeType.Interactive))
                                    ?? transitionInDirection.First();


            long nextNodeId = edgeAndTransition.Edge.To;
            RawWorldGraphNode? nextNode = _worldDataProvider.GetNode(nextNodeId);

            if (nextNode == null)
            {
                yield break;
            }

            current = nextNode;
            Map? currentMap = _worldDataProvider.GetMapOfNode(current);

            yield return current.Cook(currentMap?.Position);
        }
    }

    PathStep ComputeStep(RawWorldGraphNode current, RawWorldGraphNode next)
    {
        Map? currentMap = _worldDataProvider.GetMapOfNode(current);
        MapNodeWithPosition currentPathNode = current.Cook(currentMap?.Position);

        RawWorldGraphEdge[] edges = _worldDataProvider.GetEdgesBetweenNodes(current.Id, next.Id).ToArray();
        RawWorldGraphEdgeTransition[] transitions = edges.SelectMany(e => e.Transitions ?? []).ToArray();
        RawWorldGraphEdgeTransition? transition = transitions.FirstOrDefault();

        return new PathStep { Node = currentPathNode, Transition = transition?.Cook() };
    }

    static bool DirectionEquals(RawWorldGraphEdgeDirection rawDirection, Direction direction) =>
        direction switch
        {
            Direction.North => rawDirection == RawWorldGraphEdgeDirection.North,
            Direction.South => rawDirection == RawWorldGraphEdgeDirection.South,
            Direction.East => rawDirection == RawWorldGraphEdgeDirection.East,
            Direction.West => rawDirection == RawWorldGraphEdgeDirection.West,
            _ => false
        };
}
