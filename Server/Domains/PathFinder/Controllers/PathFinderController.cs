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
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    public PathFinderController(WorldGraphServiceFactory worldGraphServiceFactory, RawMapPositionsServiceFactory rawMapPositionsServiceFactory, ILoggerFactory loggerFactory)
    {
        _worldGraphServiceFactory = worldGraphServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _loggerFactory = loggerFactory;
    }

    public async Task<FindPathResponse> FindPath(long fromMapId, long toMapId, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateService(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateService(cancellationToken: cancellationToken);

        AStarService aStarService = new(worldGraphService, rawMapPositionsService, _loggerFactory.CreateLogger<AStarService>());
        throw new NotImplementedException();
    }
}
