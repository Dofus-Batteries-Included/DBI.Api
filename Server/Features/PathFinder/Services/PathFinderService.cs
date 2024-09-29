using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Models;
using Server.Features.PathFinder.Services.PathFinding;
using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Services;

class PathFinderService
{
    readonly IPathFindingStrategy _pathFindingStrategy;
    readonly RawWorldGraphService _rawWorldGraphService;
    readonly MapsService _mapsService;

    public PathFinderService(IPathFindingStrategy pathFindingStrategy, RawWorldGraphService rawWorldGraphService, MapsService mapsService)
    {
        _pathFindingStrategy = pathFindingStrategy;
        _rawWorldGraphService = rawWorldGraphService;
        _mapsService = mapsService;
    }

    public Path? GetShortestPath(RawWorldGraphNode sourceNode, RawWorldGraphNode targetNode)
    {
        IReadOnlyList<RawWorldGraphNode>? rawPath = _pathFindingStrategy.ComputePath(sourceNode, targetNode);
        if (rawPath == null)
        {
            return null;
        }

        Map? sourceMap = _mapsService.GetMap(sourceNode);
        Map? targetMap = _mapsService.GetMap(targetNode);

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
            From = new PathMap { MapId = sourceNode.MapId, ZoneId = sourceNode.ZoneId, MapPosition = sourceMap?.Position, WorldGraphNodeId = sourceNode.Id },
            To = new PathMap { MapId = targetNode.MapId, ZoneId = targetNode.ZoneId, MapPosition = targetMap?.Position, WorldGraphNodeId = targetNode.Id },
            Steps = steps
        };
    }

    PathStep ComputeStep(RawWorldGraphNode current, RawWorldGraphNode next)
    {
        Map? currentMap = _mapsService.GetMap(current);
        PathMap currentPathMap = new() { MapId = current.MapId, ZoneId = current.ZoneId, MapPosition = currentMap?.Position, WorldGraphNodeId = current.Id };

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
}
