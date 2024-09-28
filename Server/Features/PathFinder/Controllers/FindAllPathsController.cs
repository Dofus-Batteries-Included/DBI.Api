using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Controllers.Responses;
using Server.Features.PathFinder.Services;
using Path = Server.Features.PathFinder.Models.Path;

namespace Server.Features.PathFinder.Controllers;

[Route("path-finder")]
[Tags("Path Finder")]
[ApiController]
public class FindAllPathsController : ControllerBase
{
    readonly WorldGraphServiceFactory _worldGraphServiceFactory;
    readonly MapsServiceFactory _mapsServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    public FindAllPathsController(WorldGraphServiceFactory worldGraphServiceFactory, MapsServiceFactory mapsServiceFactory, ILoggerFactory loggerFactory)
    {
        _worldGraphServiceFactory = worldGraphServiceFactory;
        _mapsServiceFactory = mapsServiceFactory;
        _loggerFactory = loggerFactory;
    }

    [HttpGet("paths")]
    public async Task<FindAllPathsResponse> FindAllPaths(long fromMapId, long toMapId, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _mapsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);

        return FindPathImpl(worldGraphService, mapsService, fromMapId, toMapId);
    }

    FindAllPathsResponse FindPathImpl(WorldGraphService worldGraphService, MapsService mapsService, long fromMapId, long toMapId)
    {
        WorldGraphNode[] fromNodes = worldGraphService.GetNodesAtMap(fromMapId).ToArray();
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode[] toNodes = worldGraphService.GetNodesAtMap(toMapId).ToArray();
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        Path[] paths = fromNodes
            .SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
            .Where(p => p.Path != null)
            .Select(p => p.Path!)
            .ToArray();

        return new FindAllPathsResponse { Paths = paths };
    }
}
