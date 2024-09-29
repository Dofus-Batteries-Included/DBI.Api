using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.Maps;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Services;
using Server.Features.PathFinder.Controllers.Requests;

namespace Server.Features.PathFinder.Services;

class NodeFinderService(RawWorldGraphService rawWorldGraphService, RawMapPositionsService rawMapPositionsService, MapsService mapsService)
{
    public IEnumerable<RawWorldGraphNode> FindNodes(FindNodeRequest request)
    {
        switch (request)
        {
            case FindNodeById findNodeById:
                RawWorldGraphNode? node = rawWorldGraphService.GetNode(findNodeById.NodeId);
                return node == null ? [] : [node];
            case FindNodeByMap findNodeByMap:
                return FindNodesImpl(findNodeByMap.MapId, findNodeByMap.CellNumber);
            case FindNodeAtPosition findNodeAtPosition:
                return FindNodesImpl(findNodeAtPosition.Position, findNodeAtPosition.CellNumber);
            default:
                return [];
        }
    }

    IEnumerable<RawWorldGraphNode> FindNodesImpl(long mapId, int? cellNumber)
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

    RawWorldGraphNode[] FindNodesImpl(Position mapPosition, int? cellNumber)
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
