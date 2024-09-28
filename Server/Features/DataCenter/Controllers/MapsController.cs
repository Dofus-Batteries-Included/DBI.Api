using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Controllers;

/// <summary>
///     Retrieve raw data in JSON files.
/// </summary>
[Route("data-center/versions/{gameVersion}/maps")]
[Tags("Maps")]
[ApiController]
public class MapsController : ControllerBase
{
    readonly MapsServiceFactory _mapsServiceFactory;

    public MapsController(MapsServiceFactory mapsServiceFactory)
    {
        _mapsServiceFactory = mapsServiceFactory;
    }

    [HttpGet("{mapId:long}")]
    public async Task<Map> GetMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService mapsService = await _mapsServiceFactory.CreateServiceAsync(gameVersion, cancellationToken);
        return mapsService.GetMap(mapId) ?? throw new NotFoundException("Could not find map.");
    }

    [HttpGet("{mapId:long}/cells/{cellNumber:int}")]
    public async Task<Cell> GetMapCell(long mapId, int cellNumber, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService mapsService = await _mapsServiceFactory.CreateServiceAsync(gameVersion, cancellationToken);
        return mapsService.GetCell(mapId, cellNumber) ?? throw new NotFoundException("Could not find cell.");
    }
}
