using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Controllers.World;

/// <summary>
///     Areas endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/areas")]
[OpenApiTag("World - Areas")]
[ApiController]
public class AreasController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    /// <summary>
    /// </summary>
    public AreasController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    /// <summary>
    ///     Get areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Area>> GetAreas(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService worldService = await _worldServiceFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetAreas() ?? throw new NotFoundException($"Could not find areas in version {gameVersion}.");
    }

    /// <summary>
    ///     Get area
    /// </summary>
    [HttpGet("{areaId:int}")]
    public async Task<Area> GetArea(int areaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService worldService = await _worldServiceFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetArea(areaId) ?? throw new NotFoundException($"Could not find area in version {gameVersion}.");
    }

    /// <summary>
    ///     Get sub areas in area
    /// </summary>
    [HttpGet("{areaId:int}/sub-areas")]
    public async Task<IEnumerable<SubArea>> GetSubAreasInSuperArea(int areaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService subAreasService = await _worldServiceFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return subAreasService.GetSubAreasInSuperArea(areaId) ?? throw new NotFoundException($"Could not find sub areas in version: {gameVersion}.");
    }
}
