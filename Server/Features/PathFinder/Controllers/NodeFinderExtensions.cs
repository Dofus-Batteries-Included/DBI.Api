using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.PathFinder;
using DBI.Server.Features.PathFinder.Controllers.Requests;

namespace DBI.Server.Features.PathFinder.Controllers;

static class NodeFinderExtensions
{
    public static IEnumerable<RawWorldGraphNode> FindNodes(this NodeFinder nodeFinder, FindNodeRequest request)
    {
        switch (request)
        {
            case FindNodeById findNodeById:
            {
                RawWorldGraphNode? node = nodeFinder.FindNodeById(findNodeById.NodeId);
                return node == null ? [] : [node];
            }
            case FindNodeByMap findNodeByMap:
            {
                if (findNodeByMap.CellNumber.HasValue)
                {
                    RawWorldGraphNode? node = nodeFinder.FindNodeByMap(findNodeByMap.MapId, findNodeByMap.CellNumber.Value);
                    return node == null ? [] : [node];
                }

                return nodeFinder.FindNodesByMap(findNodeByMap.MapId);
            }
            case FindNodeAtPosition findNodeAtPosition:
            {
                return nodeFinder.FindNodesAtPosition(findNodeAtPosition.Position, findNodeAtPosition.CellNumber);
            }
            default:
                return [];
        }
    }
}
