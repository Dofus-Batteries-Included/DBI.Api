using DBI.Server.Common.Models;
using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Raw.Models.WorldGraphs;
using DBI.Server.Features.DataCenter.Raw.Services.WorldGraphs;
using DBI.Server.Features.DataCenter.Services;
using DBI.Server.Features.PathFinder.Services;
using DBI.Server.Features.PathFinder.Services.PathFinding;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues;

namespace DBI.Server.Features.TreasureSolver.Services;

/// <summary>
///     Solve treasure hunts.
/// </summary>
public class TreasureSolverService(
    FindCluesService findCluesService,
    RawWorldGraphServiceFactory rawWorldGraphServiceFactory,
    WorldServiceFactory worldServiceFactory,
    ILoggerFactory loggerFactory
)
{
    /// <summary>
    ///     Find the next node in the treasure hunt.
    ///     The next node is the first one containing the clue <see cref="clueId" /> when moving from the node <see cref="startNodeId" /> in direction <see cref="direction" /> for up to
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
        CancellationToken cancellationToken = default
    )
    {
        RawWorldGraphService rawWorldGraphService = await rawWorldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        RawWorldGraphNode? startNode = rawWorldGraphService.GetNode(startNodeId);
        if (startNode == null)
        {
            return new FindNextNodeContainingClueResult(false, null, null);
        }

        AStar pathFindingStrategy = new(rawWorldGraphService, mapsService, loggerFactory.CreateLogger<AStar>());
        PathFinderService pathFinderService = new(pathFindingStrategy, rawWorldGraphService, mapsService);

        int distance = 1;
        foreach (MapNodeWithPosition node in pathFinderService.EnumerateNodesInDirection(startNode, direction))
        {
            IReadOnlyCollection<Clue> clues = await findCluesService.FindCluesInMapAsync(node.MapId);
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
