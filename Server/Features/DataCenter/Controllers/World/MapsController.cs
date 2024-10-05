using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers.World;

/// <summary>
///     Maps endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/maps")]
[OpenApiTag("World - Maps")]
[ApiController]
public class MapsController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    /// <summary>
    /// </summary>
    public MapsController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    /// <summary>
    ///     Get map
    /// </summary>
    [HttpGet("{mapId:long}")]
    public async Task<Map> GetMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetMap(mapId) ?? throw new NotFoundException($"Could not find map in version {gameVersion}.");
    }

    /// <summary>
    ///     Get map cells
    /// </summary>
    [HttpGet("{mapId:long}/cells")]
    public async Task<IEnumerable<MapCell>> GetMapCells(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetCells(mapId) ?? throw new NotFoundException($"Could not find map cells in version {gameVersion}.");
    }

    /// <summary>
    ///     Get map cell
    /// </summary>
    [HttpGet("{mapId:long}/cells/{cellNumber:int}")]
    public async Task<MapCell> GetMapCell(long mapId, int cellNumber, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        MapsService worldService = await _worldServiceFactory.CreateMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetCell(mapId, cellNumber) ?? throw new NotFoundException($"Could not find map cell in version {gameVersion}.");
    }

    /// <summary>
    ///     Get nodes in map
    /// </summary>
    [HttpGet("{mapId:long}/nodes")]
    public async Task<IEnumerable<MapNode>> GetNodesInMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        WorldGraphService worldService = await _worldServiceFactory.CreateWorldGraphServiceAsync(gameVersion, cancellationToken);
        return worldService.GetNodesInMap(mapId) ?? throw new NotFoundException($"Could not find nodes in version {gameVersion}.");
    }

    /// <summary>
    ///     Get transitions from map
    /// </summary>
    [HttpGet("{mapId:long}/transitions/outgoing")]
    public async Task<IEnumerable<MapTransition>> GetTransitionsFromMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        WorldGraphService worldService = await _worldServiceFactory.CreateWorldGraphServiceAsync(gameVersion, cancellationToken);
        return worldService.GetTransitionsFromMap(mapId) ?? throw new NotFoundException($"Could not find transitions in version {gameVersion}.");
    }

    /// <summary>
    ///     Get transitions to map
    /// </summary>
    [HttpGet("{mapId:long}/transitions/incoming")]
    public async Task<IEnumerable<MapTransition>> GetTransitionsToMap(long mapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        WorldGraphService worldService = await _worldServiceFactory.CreateWorldGraphServiceAsync(gameVersion, cancellationToken);
        return worldService.GetTransitionsToMap(mapId) ?? throw new NotFoundException($"Could not find transitions in version {gameVersion}.");
    }
}
