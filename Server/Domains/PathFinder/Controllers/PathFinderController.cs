﻿using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Domains.DataCenter.Models.Maps;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Raw.Services.WorldGraphs;
using Server.Domains.DataCenter.Services;
using Server.Domains.PathFinder.Controllers.Responses;
using Server.Domains.PathFinder.Services;
using Path = Server.Domains.PathFinder.Models.Path;

namespace Server.Domains.PathFinder.Controllers;

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

    [HttpGet("path")]
    public async Task<FindPathResponse> FindPath(long fromMapId, int fromMapCellNumber, long toMapId, int toMapCellNumber, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _mapsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);

        return FindPathImpl(worldGraphService, mapsService, fromMapId, fromMapCellNumber, toMapId, toMapCellNumber);
    }

    FindPathResponse FindPathImpl(WorldGraphService worldGraphService, MapsService mapsService, long fromMapId, int fromMapCellNumber, long toMapId, int toMapCellNumber)
    {
        Cell? fromCell = mapsService.GetCell(fromMapId, fromMapCellNumber);
        if (fromCell == null)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode? fromNode = FindNode(worldGraphService, fromMapId, fromCell);
        if (fromNode == null)
        {
            throw new NotFoundException("Could not find start position.");
        }

        Cell? toCell = mapsService.GetCell(toMapId, toMapCellNumber);
        if (toCell == null)
        {
            throw new NotFoundException("Could not find end position.");
        }

        WorldGraphNode? toNode = FindNode(worldGraphService, toMapId, toCell);
        if (toNode == null)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());
        Path? path = aStarService.GetShortestPath(fromNode, toNode);

        return new FindPathResponse { FoundPath = path != null, Steps = path?.Steps };
    }

    static WorldGraphNode FindNode(WorldGraphService worldGraphService, long mapId, Cell cell)
    {
        int zone = cell.LinkedZone / 16;
        return worldGraphService.GetNode(mapId, zone) ?? worldGraphService.GetNodesAtMap(mapId).First();
    }
}
