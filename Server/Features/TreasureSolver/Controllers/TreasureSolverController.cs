﻿using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services;
using DBI.Server.Common.Exceptions;
using DBI.Server.Features.PathFinder.Controllers.Requests;
using DBI.Server.Features.PathFinder.Services;
using DBI.Server.Features.TreasureSolver.Controllers.Requests;
using DBI.Server.Features.TreasureSolver.Controllers.Responses;
using DBI.Server.Features.TreasureSolver.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.TreasureSolver.Controllers;

/// <summary>
///     Treasure solver endpoints
/// </summary>
[Route("treasure-solver")]
[OpenApiTag("Treasure Solver")]
[ApiController]
public class TreasureSolverController : ControllerBase
{
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly WorldServiceFactory _worldServiceFactory;
    readonly RawWorldGraphServiceFactory _rawWorldGraphServiceFactory;
    readonly TreasureSolverService _solver;

    /// <summary>
    /// </summary>
    public TreasureSolverController(
        RawWorldGraphServiceFactory rawWorldGraphServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
        WorldServiceFactory worldServiceFactory,
        TreasureSolverService solver
    )
    {
        _rawWorldGraphServiceFactory = rawWorldGraphServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _worldServiceFactory = worldServiceFactory;
        _solver = solver;
    }

    /// <summary>
    ///     Find nodes
    /// </summary>
    /// <remarks>
    ///     The endpoint is mostly used to understand how <see cref="FindNodeRequest" />s work. The actual clue search is performed by <see cref="FindNextClue" />.
    /// </remarks>
    [HttpPost("find-nodes")]
    public async Task<IEnumerable<MapNodeWithPosition>> FindNodes(FindNodeRequest request, CancellationToken cancellationToken = default)
    {
        RawWorldGraphService rawWorldGraphService = await _rawWorldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        NodeFinderService nodeFinder = new(rawWorldGraphService, rawMapPositionsService, mapsService);

        return nodeFinder.FindNodes(request).Select(n => n.Cook(mapsService.GetMap(n.MapId)?.Position));
    }

    /// <summary>
    ///     Find next clue
    /// </summary>
    [HttpPost("find-next-clue")]
    public async Task<ActionResult<FindNextMapResponse>> FindNextClue(FindNextClueRequest request, CancellationToken cancellationToken)
    {
        RawWorldGraphService rawWorldGraphService = await _rawWorldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        NodeFinderService nodeFinder = new(rawWorldGraphService, rawMapPositionsService, mapsService);
        RawWorldGraphNode[]? nodes = nodeFinder.FindNodes(request.Start).ToArray();
        if (nodes == null)
        {
            throw new NotFoundException("Could not find starting position.");
        }

        foreach (RawWorldGraphNode node in nodes)
        {
            FindNextNodeContainingClueResult result = await _solver.FindNextNodeContainingClueAsync(node.Id, request.Direction, request.ClueId, cancellationToken);
            if (result.Found)
            {
                return new FindNextMapResponse { Found = true, Map = result.Map, Distance = result.Distance };
            }
        }

        return new FindNextMapResponse { Found = false };
    }
}
