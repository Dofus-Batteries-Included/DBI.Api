using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services;

namespace DBI.PathFinder.DataProviders;

class WorldDataFromRawServices : IWorldDataProvider
{
    readonly RawWorldGraphService _rawWorldGraphService;
    readonly MapsService _mapsService;

    public WorldDataFromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService)
    {
        _rawWorldGraphService = rawWorldGraphService;
        _mapsService = mapsService;
    }

    public RawWorldGraphNode? GetNode(long nodeId) => _rawWorldGraphService.GetNode(nodeId);
    public Map? GetMap(long mapId) => _mapsService.GetMap(mapId);
    public Map? GetMapOfNode(RawWorldGraphNode node) => _mapsService.GetMap(node);
    public IEnumerable<RawWorldGraphNode> GetNodesInMap(long mapId) => _rawWorldGraphService.GetNodesInMap(mapId);
    public IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long nodeId) => _rawWorldGraphService.GetEdgesFrom(nodeId);
    public IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long fromNodeId, long toNodeId) => _rawWorldGraphService.GetEdges(fromNodeId, toNodeId);
    public MapCell? GetCell(long mapId, int cellNumber) => _mapsService.GetCell(mapId, cellNumber);
    public IEnumerable<Map> GetMapsAtPosition(Position mapPosition) => _mapsService.GetMapsAtPosition(mapPosition);

    public RawWorldGraphNode? GetNodeInMapAtCell(long mapId, int cellNumber)
    {
        MapCell? cell = _mapsService.GetCell(mapId, cellNumber);
        if (cell == null)
        {
            return null;
        }

        int zone = cell.LinkedZone / 16;
        return _rawWorldGraphService.GetNode(mapId, zone);
    }
}
