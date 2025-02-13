﻿using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.Builders;
using DBI.PathFinder.DataProviders;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues;

namespace DBI.Server.Features.TreasureSolver.Services;

/// <summary>
///     Solve treasure hunts.
/// </summary>
public class TreasureSolverService(
    FindCluesService findCluesService,
    RawWorldGraphServiceFactory rawWorldGraphServiceFactory,
    RawMapsServiceFactory rawMapsServiceFactory,
    RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
    ILoggerFactory loggerFactory
)
{
    /// <summary>
    ///     Find the next node in the treasure hunt.
    ///     The next node is the first one containing the clue <c>clueId</c> when moving from the node <c>startNodeId</c> in direction <c>direction</c> for up to
    ///     10
    ///     steps.
    /// </summary>
    /// <remarks>
    ///     This method is the most reliable: it starts from a node and goes through the graph.
    /// </remarks>
    public async Task<FindNextNodeContainingClueResult> FindNextNodeContainingClueAsync(
        long startNodeId,
        Direction direction,
        int clueId,
        string version = "latest",
        CancellationToken cancellationToken = default
    )
    {
        RawWorldGraphService rawWorldGraphService = await rawWorldGraphServiceFactory.CreateServiceAsync(version, cancellationToken);
        RawMapsService rawMapsService = await rawMapsServiceFactory.CreateServiceAsync(version, cancellationToken);
        RawMapPositionsService rawMapPositionsService = await rawMapPositionsServiceFactory.CreateServiceAsync(version, cancellationToken);

        RawWorldGraphNode? startNode = rawWorldGraphService.GetNode(startNodeId);
        if (startNode == null)
        {
            return new FindNextNodeContainingClueResult(false, null, null);
        }

        IWorldDataProvider worldData = WorldDataBuilder.FromRawServices(rawWorldGraphService, rawMapsService, rawMapPositionsService).Build();
        DBI.PathFinder.PathFinder pathFinder = new(worldData, loggerFactory.CreateLogger("PathFinder"));

        int distance = 1;
        foreach (MapNodeWithPosition node in pathFinder.EnumerateNodesInDirection(startNode, direction))
        {
            IReadOnlyCollection<Clue> clues = await findCluesService.FindCluesInMapAsync(node.MapId, cancellationToken);
            if (clues.Any(c => c.ClueId == clueId))
            {
                return new FindNextNodeContainingClueResult(true, node, distance);
            }

            distance++;
        }

        return new FindNextNodeContainingClueResult(false, null, null);
    }
}

/// <summary>
///     The result of <see cref="TreasureSolverService.FindNextNodeContainingClueAsync" />
/// </summary>
public readonly record struct FindNextNodeContainingClueResult(bool Found, MapNodeWithPosition? Map, int? Distance);
