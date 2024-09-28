using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Controllers.World;

/// <summary>
///     Retrieve raw data in JSON files.
/// </summary>
[Route("data-center/versions/{gameVersion}/world/maps")]
[Tags("World - Maps")]
[ApiController]
public class MapsController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    public MapsController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    [HttpGet("{mapId:long}")]
    public async Task<Map> GetMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetMap(mapId) ?? throw new NotFoundException($"Could not find map in version {gameVersion}.");
    }

    [HttpGet("{mapId:long}/cells")]
    public async Task<IEnumerable<Cell>> GetMapCells(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetCells(mapId) ?? throw new NotFoundException($"Could not find map cells in version {gameVersion}.");
    }

    [HttpGet("{mapId:long}/cells/{cellNumber:int}")]
    public async Task<Cell> GetMapCell(long mapId, int cellNumber, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetCell(mapId, cellNumber) ?? throw new NotFoundException($"Could not find map cell in version {gameVersion}.");
    }
}
