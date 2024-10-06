using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using FluentAssertions;
using Moq;
using Test.PathFinder.Fake;

namespace Test.PathFinder;

[TestClass]
public class PathFinderTest
{
    Mock<IWorldDataProvider> _worldDataProviderMock = null!;
    DBI.PathFinder.PathFinder _pathFinder = null!;

    [TestInitialize]
    public void Initialize()
    {
        _worldDataProviderMock = new Mock<IWorldDataProvider>();
        _pathFinder = new DBI.PathFinder.PathFinder(_worldDataProviderMock.Object);
    }

    [TestMethod]
    public void ShouldEnumerateNodesInDirection()
    {
        Map map1 = FakeMap.Create();
        Map map2 = FakeMap.Create();
        Map map3 = FakeMap.Create();

        RawWorldGraphNode node1 = FakeRawWorldGraphNode.Create(map1);
        RawWorldGraphNode node2 = FakeRawWorldGraphNode.Create(map2);
        RawWorldGraphNode node3 = FakeRawWorldGraphNode.Create(map3);

        RawWorldGraphEdge edge12 = new() { From = node1.Id, To = node2.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.East }] };
        RawWorldGraphEdge edge23 = new() { From = node2.Id, To = node3.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.East }] };

        _worldDataProviderMock.Setup(p => p.GetEdgesFromNode(node1.Id)).Returns([edge12]);
        _worldDataProviderMock.Setup(p => p.GetEdgesFromNode(node2.Id)).Returns([edge23]);
        _worldDataProviderMock.Setup(p => p.GetNode(node2.Id)).Returns(node2);
        _worldDataProviderMock.Setup(p => p.GetNode(node3.Id)).Returns(node3);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(node2)).Returns(map2);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(node3)).Returns(map3);

        IEnumerable<MapNodeWithPosition> nodes = _pathFinder.EnumerateNodesInDirection(node1, Direction.East);

        nodes.Should()
            .BeEquivalentTo(
                [
                    new MapNodeWithPosition
                    {
                        NodeId = node2.Id,
                        MapId = map2.MapId,
                        ZoneId = node2.ZoneId,
                        MapPosition = map2.Position
                    },
                    new MapNodeWithPosition
                    {
                        NodeId = node3.Id,
                        MapId = map3.MapId,
                        ZoneId = node3.ZoneId,
                        MapPosition = map3.Position
                    }
                ]
            );
    }
}
