using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.PathFinder.DataProviders;

public interface IWorldDataProvider
{
    RawWorldGraphNode? GetNode(long nextNodeId);
    Map? GetMapOfNode(RawWorldGraphNode sourceNode);
    IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long currentId);
    IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long currentId, long nextId);
}
