using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services.World;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Sub Areas endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/sub-areas")]
[OpenApiTag("World - Sub Areas")]
[ApiController]
public class SubAreasController : ControllerBase
{
    readonly WorldServicesFactory _worldServicesFactory;

    /// <summary>
    /// </summary>
    public SubAreasController(WorldServicesFactory worldServicesFactory)
    {
        _worldServicesFactory = worldServicesFactory;
    }

    /// <summary>
    ///     Get sub areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<SubArea>> GetSubArea(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService worldService = await _worldServicesFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetSubAreas() ?? throw new NotFoundException($"Could not find sub areas in version {gameVersion}.");
    }

    /// <summary>
    ///     Get sub area
    /// </summary>
    [HttpGet("{subAreaId:int}")]
    public async Task<SubArea> GetSubArea(int subAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService worldService = await _worldServicesFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetSubArea(subAreaId) ?? throw new NotFoundException($"Could not find sub area in version {gameVersion}.");
    }
}
