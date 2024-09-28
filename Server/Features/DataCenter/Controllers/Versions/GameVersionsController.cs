﻿using Microsoft.AspNetCore.Mvc;
using Server.Features.DataCenter.Controllers.Versions.Responses;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Controllers.Versions;

/// <summary>
///     Retrieve the available game versions.
/// </summary>
[Route("data-center/game-versions")]
[Tags("Game versions")]
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
        string latestVersion = await _repository.GetLatestVersionAsync();
        IReadOnlyCollection<string> versions = await _repository.GetAvailableVersionsAsync();
        return new GetAvailableVersionsResponse
        {
            Latest = latestVersion,
            Versions = versions
        };
    }
}
