using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.PathFinder.DataProviders;

/// <summary>
///     Provide data about the game world.
///     Used by <see cref="PathFinder" /> and <see cref="NodeFinder" />
/// </summary>
public interface IWorldDataProvider
{
    RawWorldGraphNode? GetNode(long nodeId);
    IEnumerable<RawWorldGraphEdge> GetEdgesFromNode(long nodeId);
    IEnumerable<RawWorldGraphEdge> GetEdgesBetweenNodes(long fromNodeId, long toNodeId);

    Map? GetMap(long mapId);
    IEnumerable<Map> GetMapsAtPosition(Position mapPosition);
    MapCell? GetCell(long mapId, int cellNumber);

    IEnumerable<RawWorldGraphNode> GetNodesInMap(long mapId);
    RawWorldGraphNode? GetNodeInMapAtCell(long mapId, int cellNumber);
    Map? GetMapOfNode(RawWorldGraphNode node);
}
