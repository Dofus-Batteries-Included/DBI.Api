using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services.World;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Areas endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/areas")]
[OpenApiTag("World - Areas")]
[ApiController]
public class AreasController : ControllerBase
{
    readonly WorldServicesFactory _worldServicesFactory;

    /// <summary>
    /// </summary>
    public AreasController(WorldServicesFactory worldServicesFactory)
    {
        _worldServicesFactory = worldServicesFactory;
    }

    /// <summary>
    ///     Get areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Area>> GetAreas(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService worldService = await _worldServicesFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetAreas() ?? throw new NotFoundException($"Could not find areas in version {gameVersion}.");
    }

    /// <summary>
    ///     Get area
    /// </summary>
    [HttpGet("{areaId:int}")]
    public async Task<Area> GetArea(int areaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService worldService = await _worldServicesFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetArea(areaId) ?? throw new NotFoundException($"Could not find area in version {gameVersion}.");
    }

    /// <summary>
    ///     Get sub areas in area
    /// </summary>
    [HttpGet("{areaId:int}/sub-areas")]
    public async Task<IEnumerable<SubArea>> GetSubAreasInSuperArea(int areaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService subAreasService = await _worldServicesFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return subAreasService.GetSubAreasInSuperArea(areaId) ?? throw new NotFoundException($"Could not find sub areas in version: {gameVersion}.");
    }
}
