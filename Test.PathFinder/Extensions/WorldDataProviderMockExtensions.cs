using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using Moq;

namespace Test.PathFinder.Extensions;

public static class WorldDataProviderMockExtensions
{
    public static void SetupNodeAndMap(this Mock<IWorldDataProvider> worldDataProviderMock, RawWorldGraphNode node, Map map)
    {
        worldDataProviderMock.Setup(p => p.GetNode(node.Id)).Returns(node);
        worldDataProviderMock.Setup(p => p.GetMap(map.MapId)).Returns(map);
        worldDataProviderMock.Setup(p => p.GetMapOfNode(node)).Returns(map);
        worldDataProviderMock.Setup(p => p.GetNodesInMap(node.MapId)).Returns([node]);
    }

    public static void SetupEdges(this Mock<IWorldDataProvider> worldDataProviderMock, params RawWorldGraphEdge[] edges)
    {
        foreach (RawWorldGraphEdge edge in edges)
        {
            worldDataProviderMock.Setup(p => p.GetEdgesFromNode(edge.From)).Returns([edge]);
            worldDataProviderMock.Setup(p => p.GetEdgesBetweenNodes(edge.From, edge.To)).Returns([edge]);
        }
    }
}
