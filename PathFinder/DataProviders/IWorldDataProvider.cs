using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.PathFinder.DataProviders;

public interface IWorldDataProvider
{
    RawWorldGraphNode? GetNode(long nodeId);
    Map? GetMap(long mapId);
    Map? GetMapOfNode(RawWorldGraphNode node);
    IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long nodeId);
    IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long fromNodeId, long toNodeId);
}
