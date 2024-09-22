﻿using Microsoft.AspNetCore.Mvc;
using Server.Common.Models;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.TreasureSolver.Controllers.Responses;
using Server.Domains.TreasureSolver.Services;

namespace Server.Domains.TreasureSolver.Controllers;

[Route("treasure-solver")]
[Tags("Treasure Solver")]
[ApiController]
public class TreasureSolverController : ControllerBase
{
    readonly MapsServiceFactory _mapsServiceFactory;
    readonly TreasureSolverService _solver;

    public TreasureSolverController(MapsServiceFactory mapServiceFactory, TreasureSolverService solver)
    {
        _solver = solver;
        _mapsServiceFactory = mapServiceFactory;
    }

    /// <summary>
    ///     Find next map
    /// </summary>
    [HttpGet("{startMapId:long}/{direction}/{clueId:int}")]
    public async Task<ActionResult<FindNextMapResponse>> FindNextMap(long startMapId, Direction direction, int clueId)
    {
        MapsService mapService = await _mapsServiceFactory.CreateService();
        MapPositions? startMap = mapService.GetMap(startMapId);
        if (startMap is null)
        {
            return BadRequest("Invalid start map.");
        }

        MapPositions? result = await _solver.FindNextMapAsync(startMap, direction, clueId);
        return new FindNextMapResponse
        {
            Found = result != null,
            MapId = result?.MapId,
            MapPosition = result != null ? new Position(result.PosX, result.PosY) : null
        };
    }

    /// <summary>
    ///     Find next position
    /// </summary>
    [HttpGet("{posX:int}/{posY:int}/{direction}/{clueId:int}")]
    public async Task<FindNextPositionResponse> FindNextPosition(int posX, int posY, Direction direction, int clueId)
    {
        Position? result = await _solver.FindNextMapAsync(new Position(posX, posY), direction, clueId);
        return new FindNextPositionResponse
        {
            Found = result != null,
            MapPosition = result
        };
    }
}
