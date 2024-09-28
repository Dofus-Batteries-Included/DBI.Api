using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Controllers.World;

[Route("data-center/versions/{gameVersion}/world/super-areas")]
[Tags("World - Super Areas")]
[ApiController]
public class SuperAreasController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    public SuperAreasController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    /// <summary>
    ///     Get super areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<SuperArea>> GetSuperArea(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SuperAreasService superAreasService = await _worldServiceFactory.CreateSuperAreasServiceAsync(gameVersion, cancellationToken);
        return superAreasService.GetSuperAreas() ?? throw new NotFoundException($"Could not find super areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get super area
    /// </summary>
    [HttpGet("{superAreaId:int}")]
    public async Task<SuperArea> GetSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SuperAreasService superAreasService = await _worldServiceFactory.CreateSuperAreasServiceAsync(gameVersion, cancellationToken);
        return superAreasService.GetSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find super area in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get areas in super area
    /// </summary>
    [HttpGet("{superAreaId:int}/areas")]
    public async Task<IEnumerable<Area>> GetAreasInSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService areasService = await _worldServiceFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return areasService.GetAreasInSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get sub areas in super area
    /// </summary>
    [HttpGet("{superAreaId:int}/sub-areas")]
    public async Task<IEnumerable<SubArea>> GetSubAreasInSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService subAreasService = await _worldServiceFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return subAreasService.GetSubAreasInSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find sub areas in version: {gameVersion}.");
    }
}
