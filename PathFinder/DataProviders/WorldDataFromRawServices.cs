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
    public IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long nodeId) => _rawWorldGraphService.GetEdgesFrom(nodeId);
    public IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long fromNodeId, long toNodeId) => _rawWorldGraphService.GetEdges(fromNodeId, toNodeId);
}
