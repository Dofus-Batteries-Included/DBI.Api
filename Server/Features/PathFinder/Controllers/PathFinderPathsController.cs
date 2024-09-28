using Microsoft.AspNetCore.Mvc;
using Server.Common.Exceptions;
using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Maps;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Controllers.Requests;
using Server.Features.PathFinder.Controllers.Responses;
using Server.Features.PathFinder.Services;

namespace Server.Features.PathFinder.Controllers;

/// <summary>
///     Path finder endpoints
/// </summary>
[Route("path-finder/path")]
[Tags("Path Finder")]
[ApiController]
public class PathFinderPathsController : ControllerBase
{
    readonly WorldGraphServiceFactory _worldGraphServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;
    readonly WorldServiceFactory _worldServiceFactory;
    readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// </summary>
    public PathFinderPathsController(
        WorldGraphServiceFactory worldGraphServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
        WorldServiceFactory worldServiceFactory,
        ILoggerFactory loggerFactory
    )
    {
        _worldGraphServiceFactory = worldGraphServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _worldServiceFactory = worldServiceFactory;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    ///     Find paths between maps identified by their IDs
    /// </summary>
    /// <remarks>
    ///     The endpoint might return multiple paths because there might be multiple nodes in the start and end maps.
    ///     A node is a subset of cells in a map that are connected, if a map has multiple nodes the endpoint will return all the paths it can find between all the pairs of nodes. <br />
    ///     Consider providing cell numbers to restrict the search to the actual nodes where the character is located.
    /// </remarks>
    [HttpGet("from/{fromMapId:long}/to/{toMapId:long}")]
    public async Task<FindPathsResponse> FindPathsFromIdToId(long fromMapId, long toMapId, [FromQuery] FindPathsRequest request, CancellationToken cancellationToken = default)
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        WorldGraphNode[] fromNodes = FindNodes(worldGraphService, mapsService, fromMapId, request.FromCellNumber);
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode[] toNodes = FindNodes(worldGraphService, mapsService, toMapId, request.ToCellNumber);
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }

    /// <summary>
    ///     Find paths between a map identified by its position and a map identified by its id
    /// </summary>
    /// <inheritdoc cref="FindPathsFromIdToId" />
    [HttpGet("from/position/{fromMapX:int}/{fromMapY:int}/to/{toMapId:long}")]
    public async Task<FindPathsResponse> FindPathsFromPositionToId(
        int fromMapX,
        int fromMapY,
        long toMapId,
        [FromQuery] FindPathsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        WorldGraphNode[] fromNodes = FindNodes(worldGraphService, rawMapPositionsService, mapsService, new Position(fromMapX, fromMapY), request.FromCellNumber);
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode[] toNodes = FindNodes(worldGraphService, mapsService, toMapId, request.ToCellNumber);
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }

    /// <summary>
    ///     Find paths between a map identified by its id and a map identified by its position
    /// </summary>
    /// <inheritdoc cref="FindPathsFromIdToId" />
    [HttpGet("from/position/{fromMapId:long}/to/position/{toMapX:int}/{toMapY:int}")]
    public async Task<FindPathsResponse> FindPathsFromIdToPosition(
        long fromMapId,
        int toMapX,
        int toMapY,
        [FromQuery] FindPathsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        WorldGraphNode[] fromNodes = FindNodes(worldGraphService, mapsService, fromMapId, request.FromCellNumber);
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode[] toNodes = FindNodes(worldGraphService, rawMapPositionsService, mapsService, new Position(toMapX, toMapY), request.ToCellNumber);
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }

    /// <summary>
    ///     Find paths between maps identified by their positions
    /// </summary>
    /// <inheritdoc cref="FindPathsFromIdToId" />
    [HttpGet("from/position/{fromMapX:int}/{fromMapY:int}/to/position/{toMapX:int}/{toMapY:int}")]
    public async Task<FindPathsResponse> FindPathsFromPositionToPosition(
        int fromMapX,
        int fromMapY,
        int toMapX,
        int toMapY,
        [FromQuery] FindPathsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        WorldGraphService worldGraphService = await _worldGraphServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        MapsService mapsService = await _worldServiceFactory.CreateMapsServiceAsync(cancellationToken: cancellationToken);

        WorldGraphNode[] fromNodes = FindNodes(worldGraphService, rawMapPositionsService, mapsService, new Position(fromMapX, fromMapY), request.FromCellNumber);
        if (fromNodes.Length == 0)
        {
            throw new NotFoundException("Could not find start position.");
        }

        WorldGraphNode[] toNodes = FindNodes(worldGraphService, rawMapPositionsService, mapsService, new Position(toMapX, toMapY), request.ToCellNumber);
        if (toNodes.Length == 0)
        {
            throw new NotFoundException("Could not find end position.");
        }

        AStarService aStarService = new(worldGraphService, mapsService, _loggerFactory.CreateLogger<AStarService>());

        return new FindPathsResponse
        {
            Paths = fromNodes.SelectMany(fromNode => toNodes.Select(toNode => new { FromNode = fromNode, ToNode = toNode, Path = aStarService.GetShortestPath(fromNode, toNode) }))
                .Where(p => p.Path != null)
                .Select(p => p.Path!)
                .ToArray()
        };
    }

    static WorldGraphNode[] FindNodes(WorldGraphService worldGraphService, MapsService mapsService, long mapId, int? cellNumber)
    {
        if (!cellNumber.HasValue)
        {
            return worldGraphService.GetNodesAtMap(mapId).ToArray();
        }

        Cell? cell = mapsService.GetCell(mapId, cellNumber.Value);
        if (cell == null)
        {
            return [];
        }

        return [FindNode(worldGraphService, mapId, cell)];
    }

    static WorldGraphNode[] FindNodes(
        WorldGraphService worldGraphService,
        RawMapPositionsService rawMapPositionsService,
        MapsService mapsService,
        Position mapPosition,
        int? cellNumber
    )
    {
        RawMapPosition[] maps = rawMapPositionsService.GetMapsAtPosition(mapPosition).ToArray();

        if (!cellNumber.HasValue)
        {
            return maps.SelectMany(m => worldGraphService.GetNodesAtMap(m.MapId)).ToArray();
        }

        var cells = maps.Select(m => new { m.MapId, Cell = mapsService.GetCell(m.MapId, cellNumber.Value) }).Where(c => c.Cell != null).ToArray();
        return cells.Select(x => FindNode(worldGraphService, x.MapId, x.Cell!)).ToArray();
    }

    static WorldGraphNode FindNode(WorldGraphService worldGraphService, long mapId, Cell cell)
    {
        int zone = cell.LinkedZone / 16;
        return worldGraphService.GetNode(mapId, zone) ?? worldGraphService.GetNodesAtMap(mapId).First();
    }
}
