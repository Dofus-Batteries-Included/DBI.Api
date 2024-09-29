using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.Maps;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Controllers.Requests;
using Server.Features.PathFinder.Controllers.Responses;
using Server.Features.PathFinder.Services;
using Server.Features.PathFinder.Services.PathFinding;

namespace Server.Features.PathFinder.Controllers;

/// <summary>
///     Path finder endpoints
/// </summary>
[Route("path-finder/path")]
[OpenApiTag("Path Finder")]
[ApiController]
public class PathFinderController : ControllerBase
{
    readonly RawWorldGraphServiceFactory _rawWorldGraphServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly WorldServiceFactory _worldServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// </summary>
    public PathFinderController(
        RawWorldGraphServiceFactory rawWorldGraphServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
        WorldServiceFactory worldServiceFactory,
        ILoggerFactory loggerFactory
    )
    {
        _rawWorldGraphServiceFactory = rawWorldGraphServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _worldServiceFactory = worldServiceFactory;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    ///     Find nodes
    /// </summary>
    /// <remarks>
    ///     The endpoint is mostly used to understand how <see cref="FindNodeRequest" />s work. The actual path finding is performed by <see cref="FindPaths" />.
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
    ///     Find paths
    /// </summary>
    /// <remarks>
    ///     The endpoint might return multiple paths because there might be multiple nodes corresponding.
    ///     A node is a subset of cells in a map that are connected, if a map has multiple nodes the endpoint will return all the paths it can find between all the pairs of nodes. <br />
    ///     Consider providing cell numbers to restrict the search to the actual nodes where the character is located.
    /// </remarks>
    [HttpPost("find-paths")]
    public async Task<FindPathsResponse> FindPaths(FindPathsRequest request, CancellationToken cancellationToken = default)
    {
        RawWorldGraphService rawWorldGraphService = await _rawWorldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        NodeFinderService nodeFinder = new(rawWorldGraphService, rawMapPositionsService, mapsService);

        RawWorldGraphNode[] fromNodes = nodeFinder.FindNodes(request.Start).ToArray();
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        RawWorldGraphNode[] toNodes = nodeFinder.FindNodes(request.End).ToArray();
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStar pathFindingStrategy = new(rawWorldGraphService, mapsService, _loggerFactory.CreateLogger<AStar>());
        PathFinderService pathFinderService = new(pathFindingStrategy, rawWorldGraphService, mapsService);

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(
                    fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = pathFinderService.GetShortestPath(fromNode, toNode) })
                )
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }
}
