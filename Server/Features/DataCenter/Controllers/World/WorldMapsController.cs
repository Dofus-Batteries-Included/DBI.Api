using DBI.Server.Common.Exceptions;
using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers.World;

/// <summary>
///     World Maps endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/world-maps")]
[OpenApiTag("World - World Maps")]
[ApiController]
public class WorldMapsController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    /// <summary>
    /// </summary>
    public WorldMapsController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    /// <summary>
    ///     Get world maps
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<WorldMap>> GetWorldMaps(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        WorldMapsService worldService = await _worldServiceFactory.CreateWorldMapsServiceAsync(gameVersion, cancellationToken);
        return worldService.GetWorldMaps() ?? throw new NotFoundException($"Could not find world maps in version {gameVersion}.");
    }

    /// <summary>
    ///     Get world map
    /// </summary>
    [HttpGet("{worldMapId:int}")]
    public async Task<WorldMap> GetWorldMap(int worldMapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        WorldMapsService worldMapsService = await _worldServiceFactory.CreateWorldMapsServiceAsync(gameVersion, cancellationToken);
        return worldMapsService.GetWorldMap(worldMapId) ?? throw new NotFoundException($"Could not find world map in version {gameVersion}.");
    }

    /// <summary>
    ///     Get super areas in world map
    /// </summary>
    [HttpGet("{worldMapId:int}/super-areas")]
    public async Task<IEnumerable<SuperArea>> GetSuperAreasInWorldMap(int worldMapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SuperAreasService superAreasService = await _worldServiceFactory.CreateSuperAreasServiceAsync(gameVersion, cancellationToken);
        return superAreasService.GetSuperAreasInWorldMap(worldMapId) ?? throw new NotFoundException($"Could not find super areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get areas in world map
    /// </summary>
    [HttpGet("{worldMapId:int}/areas")]
    public async Task<IEnumerable<Area>> GetAreasInWorldMap(int worldMapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService areasService = await _worldServiceFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return areasService.GetAreasInWorldMap(worldMapId) ?? throw new NotFoundException($"Could not find areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get sub areas in world map
    /// </summary>
    [HttpGet("{worldMapId:int}/sub-areas")]
    public async Task<IEnumerable<SubArea>> GetSubAreasInWorldMap(int worldMapId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService subAreasService = await _worldServiceFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return subAreasService.GetSubAreasInWorldMap(worldMapId) ?? throw new NotFoundException($"Could not find sub areas in version: {gameVersion}.");
    }
}
