using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services.World;
using DBI.PathFinder;
using DBI.PathFinder.Builders;
using DBI.PathFinder.DataProviders;
using DBI.Server.Common.Exceptions;
using DBI.Server.Features.PathFinder.Controllers.Requests;
using DBI.Server.Features.PathFinder.Controllers.Responses;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.PathFinder.Controllers;

/// <summary>
///     Path finder endpoints
/// </summary>
[Route("path-finder/path")]
[OpenApiTag("Path Finder")]
[ApiController]
public class PathFinderController : ControllerBase
{
    readonly RawWorldGraphServiceFactory _rawWorldGraphServiceFactory;
    readonly RawMapsServiceFactory _rawMapsServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly WorldServicesFactory _worldServicesFactory;
    readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// </summary>
    public PathFinderController(
        RawWorldGraphServiceFactory rawWorldGraphServiceFactory,
        RawMapsServiceFactory rawMapsServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
        WorldServicesFactory worldServicesFactory,
        ILoggerFactory loggerFactory
    )
    {
        _rawWorldGraphServiceFactory = rawWorldGraphServiceFactory;
        _rawMapsServiceFactory = rawMapsServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _worldServicesFactory = worldServicesFactory;
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
        RawMapsService rawMapsService = await _rawMapsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        IWorldDataProvider worldData = WorldDataBuilder.FromRawServices(rawWorldGraphService, rawMapsService, rawMapPositionsService).Build();

        MapsService mapsService = await _worldServicesFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        NodeFinder nodeFinder = new(worldData);

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
        RawMapsService rawMapsService = await _rawMapsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        IWorldDataProvider worldData = WorldDataBuilder.FromRawServices(rawWorldGraphService, rawMapsService, rawMapPositionsService).Build();

        NodeFinder nodeFinder = new(worldData);

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

        DBI.PathFinder.PathFinder pathFinder = new(worldData, _loggerFactory.CreateLogger("PathFinder"));

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = pathFinder.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }
}
