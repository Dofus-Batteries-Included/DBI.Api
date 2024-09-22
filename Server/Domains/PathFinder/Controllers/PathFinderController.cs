using Microsoft.AspNetCore.Mvc;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.WorldGraphs;
using Server.Domains.PathFinder.Controllers.Responses;
using Server.Domains.PathFinder.Services;

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

    public async Task<FindPathResponse> FindPath(long fromMapId, long toMapId, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateService(cancellationToken: cancellationToken);
        MapsService mapsService = await _mapsServiceFactory.CreateService(cancellationToken: cancellationToken);

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());
        throw new NotImplementedException();
    }
}
