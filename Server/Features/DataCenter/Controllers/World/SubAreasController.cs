using DBI.Server.Common.Exceptions;
using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers.World;

/// <summary>
///     Sub Areas endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/sub-areas")]
[OpenApiTag("World - Sub Areas")]
[ApiController]
public class SubAreasController : ControllerBase
{
    readonly WorldServiceFactory _worldServiceFactory;

    /// <summary>
    /// </summary>
    public SubAreasController(WorldServiceFactory worldServiceFactory)
    {
        _worldServiceFactory = worldServiceFactory;
    }

    /// <summary>
    ///     Get sub areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<SubArea>> GetSubArea(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService worldService = await _worldServiceFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetSubAreas() ?? throw new NotFoundException($"Could not find sub areas in version {gameVersion}.");
    }

    /// <summary>
    ///     Get sub area
    /// </summary>
    [HttpGet("{subAreaId:int}")]
    public async Task<SubArea> GetSubArea(int subAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService worldService = await _worldServiceFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return worldService.GetSubArea(subAreaId) ?? throw new NotFoundException($"Could not find sub area in version {gameVersion}.");
    }
}
