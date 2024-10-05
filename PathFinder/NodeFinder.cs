using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Models;

namespace DBI.PathFinder;

public class NodeFinder
{
    readonly IWorldDataProvider _worldDataProvider;

    internal NodeFinder(IWorldDataProvider worldDataProvider)
    {
        _worldDataProvider = worldDataProvider;
    }

    public IEnumerable<RawWorldGraphNode> FindNodes(FindNodeRequest request)
    {
        switch (request)
        {
            case FindNodeById findNodeById:
                RawWorldGraphNode? node = _worldDataProvider.GetNode(findNodeById.NodeId);
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
            return _worldDataProvider.GetNodesInMap(mapId);
        }

        MapCell? cell = _worldDataProvider.GetCell(mapId, cellNumber.Value);
        if (cell == null)
        {
            return [];
        }

        return [FindNode(_worldDataProvider, mapId, cell)];
    }

    RawWorldGraphNode[] FindNodesImpl(Position mapPosition, int? cellNumber)
    {
        Map[] maps = _worldDataProvider.GetMapsAtPosition(mapPosition).ToArray();

        if (!cellNumber.HasValue)
        {
            return maps.SelectMany(m => _worldDataProvider.GetNodesInMap(m.MapId)).ToArray();
        }

        var cells = maps.Select(m => new { m.MapId, Cell = _worldDataProvider.GetCell(m.MapId, cellNumber.Value) }).Where(c => c.Cell != null).ToArray();
        return cells.Select(x => FindNode(_worldDataProvider, x.MapId, x.Cell!)).ToArray();
    }

    static RawWorldGraphNode FindNode(IWorldDataProvider worldDataProvider, long mapId, MapCell mapCell) =>
        worldDataProvider.GetNodeInMapAtCell(mapId, mapCell.CellNumber) ?? worldDataProvider.GetNodesInMap(mapId).First();
}
