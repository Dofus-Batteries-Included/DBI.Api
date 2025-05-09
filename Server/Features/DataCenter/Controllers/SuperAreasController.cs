﻿using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services.World;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Super Areas endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/world/super-areas")]
[OpenApiTag("World - Super Areas")]
[ApiController]
public class SuperAreasController : ControllerBase
{
    readonly WorldServicesFactory _worldServicesFactory;

    /// <summary>
    /// </summary>
    public SuperAreasController(WorldServicesFactory worldServicesFactory)
    {
        _worldServicesFactory = worldServicesFactory;
    }

    /// <summary>
    ///     Get super areas
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<SuperArea>> GetSuperArea(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SuperAreasService superAreasService = await _worldServicesFactory.CreateSuperAreasServiceAsync(gameVersion, cancellationToken);
        return superAreasService.GetSuperAreas() ?? throw new NotFoundException($"Could not find super areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get super area
    /// </summary>
    [HttpGet("{superAreaId:int}")]
    public async Task<SuperArea> GetSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SuperAreasService superAreasService = await _worldServicesFactory.CreateSuperAreasServiceAsync(gameVersion, cancellationToken);
        return superAreasService.GetSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find super area in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get areas in super area
    /// </summary>
    [HttpGet("{superAreaId:int}/areas")]
    public async Task<IEnumerable<Area>> GetAreasInSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        AreasService areasService = await _worldServicesFactory.CreateAreasServiceAsync(gameVersion, cancellationToken);
        return areasService.GetAreasInSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find areas in version: {gameVersion}.");
    }

    /// <summary>
    ///     Get sub areas in super area
    /// </summary>
    [HttpGet("{superAreaId:int}/sub-areas")]
    public async Task<IEnumerable<SubArea>> GetSubAreasInSuperArea(int superAreaId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        SubAreasService subAreasService = await _worldServicesFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken);
        return subAreasService.GetSubAreasInSuperArea(superAreaId) ?? throw new NotFoundException($"Could not find sub areas in version: {gameVersion}.");
    }
}
