using DBI.DataCenter.Raw;
using DBI.Server.Features.DataCenter.Controllers.Responses;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Retrieve the available game versions.
/// </summary>
[Route("data-center/game-versions")]
[OpenApiTag("Game versions")]
[ApiController]
public class GameVersionsController : ControllerBase
{
    readonly IRawDataRepository _repository;

    /// <summary>
    /// </summary>
    public GameVersionsController(IRawDataRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    ///     Get available versions
    /// </summary>
    [HttpGet]
    public async Task<GetAvailableVersionsResponse> GetAvailableVersions()
    {
        string? latestVersion = await _repository.GetLatestVersionAsync();
        IReadOnlyCollection<string> versions = await _repository.GetAvailableVersionsAsync();
        return new GetAvailableVersionsResponse
        {
            Latest = latestVersion,
            Versions = versions
        };
    }
}
