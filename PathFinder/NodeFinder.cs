using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;

namespace DBI.PathFinder;

public class NodeFinder(IWorldDataProvider worldDataProvider)
{
    public RawWorldGraphNode? FindNodeById(long nodeId) => worldDataProvider.GetNode(nodeId);

    public RawWorldGraphNode? FindNodeByMap(long mapId, int cellNumber)
    {
        MapCell? cell = worldDataProvider.GetCell(mapId, cellNumber);
        return cell == null ? null : FindNode(worldDataProvider, mapId, cell);
    }

    public IEnumerable<RawWorldGraphNode> FindNodesByMap(long mapId) => worldDataProvider.GetNodesInMap(mapId);

    public IEnumerable<RawWorldGraphNode> FindNodesAtPosition(Position mapPosition, int? cellNumber = null)
    {
        Map[] maps = worldDataProvider.GetMapsAtPosition(mapPosition).ToArray();

        if (!cellNumber.HasValue)
        {
            return maps.SelectMany(m => worldDataProvider.GetNodesInMap(m.MapId)).ToArray();
        }

        var cells = maps.Select(m => new { m.MapId, Cell = worldDataProvider.GetCell(m.MapId, cellNumber.Value) }).Where(c => c.Cell != null).ToArray();
        return cells.Select(x => FindNode(worldDataProvider, x.MapId, x.Cell!)).ToArray();
    }

    static RawWorldGraphNode FindNode(IWorldDataProvider worldDataProvider, long mapId, MapCell mapCell) =>
        worldDataProvider.GetNodeInMapAtCell(mapId, mapCell.CellNumber) ?? worldDataProvider.GetNodesInMap(mapId).First();
}
