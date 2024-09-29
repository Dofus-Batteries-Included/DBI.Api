using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Server.Common.Exceptions;
using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
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
public class PathFinderPathsController : ControllerBase
{
    readonly RawWorldGraphServiceFactory _rawWorldGraphServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly WorldServiceFactory _worldServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// </summary>
    public PathFinderPathsController(
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

        return FindNodesImpl(rawWorldGraphService, rawMapPositionsService, mapsService, request).Select(n => n.Cook(mapsService.GetMap(n.MapId)?.Position));
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
    public async Task<FindPathsResponse> FindNodes(FindPathsRequest request, CancellationToken cancellationToken = default)
    {
        RawWorldGraphService rawWorldGraphService = await _rawWorldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        RawWorldGraphNode[] fromNodes = FindNodesImpl(rawWorldGraphService, rawMapPositionsService, mapsService, request.Start).ToArray();
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        RawWorldGraphNode[] toNodes = FindNodesImpl(rawWorldGraphService, rawMapPositionsService, mapsService, request.End).ToArray();
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

    IEnumerable<RawWorldGraphNode> FindNodesImpl(
        RawWorldGraphService rawWorldGraphService,
        RawMapPositionsService rawMapPositionsService,
        MapsService mapsService,
        FindNodeRequest request
    )
    {
        switch (request)
        {
            case FindNodeById findNodeById:
                RawWorldGraphNode? node = rawWorldGraphService.GetNode(findNodeById.NodeId);
                return node == null ? [] : [node];
            case FindNodeByMap findNodeByMap:
                return FindNodesImpl(rawWorldGraphService, mapsService, findNodeByMap.MapId, findNodeByMap.CellNumber);
            case FindNodeAtPosition findNodeAtPosition:
                return FindNodesImpl(rawWorldGraphService, rawMapPositionsService, mapsService, findNodeAtPosition.Position, findNodeAtPosition.CellNumber);
            default:
                return [];
        }
    }

    static IEnumerable<RawWorldGraphNode> FindNodesImpl(RawWorldGraphService rawWorldGraphService, MapsService mapsService, long mapId, int? cellNumber)
    {
        if (!cellNumber.HasValue)
        {
            return rawWorldGraphService.GetNodesInMap(mapId);
        }

        MapCell? cell = mapsService.GetCell(mapId, cellNumber.Value);
        if (cell == null)
        {
            return [];
        }

        return [FindNode(rawWorldGraphService, mapId, cell)];
    }

    static RawWorldGraphNode[] FindNodesImpl(
        RawWorldGraphService rawWorldGraphService,
        RawMapPositionsService rawMapPositionsService,
        MapsService mapsService,
        Position mapPosition,
        int? cellNumber
    )
    {
        RawMapPosition[] maps = rawMapPositionsService.GetMapsAtPosition(mapPosition).ToArray();

        if (!cellNumber.HasValue)
        {
            return maps.SelectMany(m => rawWorldGraphService.GetNodesInMap(m.MapId)).ToArray();
        }

        var cells = maps.Select(m => new { m.MapId, Cell = mapsService.GetCell(m.MapId, cellNumber.Value) }).Where(c => c.Cell != null).ToArray();
        return cells.Select(x => FindNode(rawWorldGraphService, x.MapId, x.Cell!)).ToArray();
    }

    static RawWorldGraphNode FindNode(RawWorldGraphService rawWorldGraphService, long mapId, MapCell mapCell)
    {
        int zone = mapCell.LinkedZone / 16;
        return rawWorldGraphService.GetNode(mapId, zone) ?? rawWorldGraphService.GetNodesInMap(mapId).First();
    }
}
