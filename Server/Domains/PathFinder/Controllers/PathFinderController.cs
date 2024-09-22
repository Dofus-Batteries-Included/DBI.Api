using Microsoft.AspNetCore.Mvc;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.WorldGraphs;
using Server.Domains.PathFinder.Controllers.Responses;
using Server.Domains.PathFinder.Utils;

namespace Server.Domains.PathFinder.Controllers;

[Route("path-finder")]
[Tags("Path Finder")]
[ApiController]
public class PathFinderController : ControllerBase
{
    readonly WorldGraphServiceFactory _worldGraphServiceFactory;
    readonly MapsServiceFactory _mapsServiceFactory;

    public PathFinderController(WorldGraphServiceFactory worldGraphServiceFactory, MapsServiceFactory mapsServiceFactory)
    {
        _worldGraphServiceFactory = worldGraphServiceFactory;
        _mapsServiceFactory = mapsServiceFactory;
    }

    public async Task<FindPathResponse> FindPath(long fromMapId, long toMapId, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateService(cancellationToken: cancellationToken);
        MapsService mapsService = await _mapsServiceFactory.CreateService(cancellationToken: cancellationToken);

        AStar aStar = new(worldGraphService, mapsService);
        throw new NotImplementedException();
    }
}
