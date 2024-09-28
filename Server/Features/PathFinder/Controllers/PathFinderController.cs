using Microsoft.AspNetCore.Mvc;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Controllers.Responses;
using Server.Features.PathFinder.Services;

namespace Server.Features.PathFinder.Controllers;

[Route("path-finder")]
[Tags("Path Finder")]
[ApiController]
public class PathFinderController : ControllerBase
{
    readonly WorldGraphServiceFactory _worldGraphServiceFactory;
    readonly MapsServiceFactory _mapsServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    public PathFinderController(WorldGraphServiceFactory worldGraphServiceFactory, MapsServiceFactory mapsServiceFactory, ILoggerFactory loggerFactory)
    {
        _worldGraphServiceFactory = worldGraphServiceFactory;
        _mapsServiceFactory = mapsServiceFactory;
        _loggerFactory = loggerFactory;
    }

    [HttpGet("paths/from/{fromMapId:long}/to/{toMapId:long}")]
    public async Task<FindPathsResponse> FindPaths(
        long fromMapId,
        long toMapId,
        int? fromCellNumber = null,
        int? toCellNumber = null,
        CancellationToken cancellationToken = default
    )
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _mapsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);

        WorldGraphNode[] fromNodes = FindNodes(worldGraphService, mapsService, fromMapId, fromCellNumber);
        WorldGraphNode[] toNodes = FindNodes(worldGraphService, mapsService, toMapId, toCellNumber);

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }

    static WorldGraphNode[] FindNodes(WorldGraphService worldGraphService, MapsService mapsService, long mapId, int? cellNumber)
    {
        if (!cellNumber.HasValue)
        {
            return worldGraphService.GetNodesAtMap(mapId).ToArray();
        }

        Cell? cell = mapsService.GetCell(mapId, cellNumber.Value);
        if (cell == null)
        {
            return [];
        }

        return [FindNode(worldGraphService, mapId, cell)];
    }

    static WorldGraphNode FindNode(WorldGraphService worldGraphService, long mapId, Cell cell)
    {
        int zone = cell.LinkedZone / 16;
        return worldGraphService.GetNode(mapId, zone) ?? worldGraphService.GetNodesAtMap(mapId).First();
    }
}
