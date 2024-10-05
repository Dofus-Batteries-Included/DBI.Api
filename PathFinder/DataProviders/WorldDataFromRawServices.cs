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

    public Map? GetMapOfNode(RawWorldGraphNode sourceNode) => _mapsService.GetMap(sourceNode);
    public RawWorldGraphNode? GetNode(long nextNodeId) => _rawWorldGraphService.GetNode(nextNodeId);
    public IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long currentId) => _rawWorldGraphService.GetEdgesFrom(currentId);
    public IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long currentId, long nextId) => _rawWorldGraphService.GetEdges(currentId, nextId);
}
