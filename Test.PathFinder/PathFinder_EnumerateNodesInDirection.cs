using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using FluentAssertions;
using Moq;
using Test.FakeData;

namespace Test.PathFinder;

[TestClass]
public class PathFinder_EnumerateNodesInDirection
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

    [TestMethod]
    public void ShouldReturnEmptyIfNoEdge()
    {
        Map map = FakeMap.Create();
        RawWorldGraphNode node = FakeRawWorldGraphNode.Create(map);
        _worldDataProviderMock.Setup(p => p.GetEdgesFromNode(node.Id)).Returns([]);

        IEnumerable<MapNodeWithPosition> nodes = _pathFinder.EnumerateNodesInDirection(node, Direction.East);

        nodes.Should().BeEmpty();
    }

    [TestMethod]
    public void ShouldNotEnumerateNodesInOtherDirections()
    {
        Map map = FakeMap.Create();
        Map mapNorth = FakeMap.Create();
        Map mapSouth = FakeMap.Create();
        Map mapEast = FakeMap.Create();
        Map mapWest = FakeMap.Create();

        RawWorldGraphNode node = FakeRawWorldGraphNode.Create(map);
        RawWorldGraphNode nodeNorth = FakeRawWorldGraphNode.Create(mapNorth);
        RawWorldGraphNode nodeSouth = FakeRawWorldGraphNode.Create(mapSouth);
        RawWorldGraphNode nodeEast = FakeRawWorldGraphNode.Create(mapEast);
        RawWorldGraphNode nodeWest = FakeRawWorldGraphNode.Create(mapWest);

        RawWorldGraphEdge edgeNorth = new() { From = node.Id, To = nodeNorth.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.North }] };
        RawWorldGraphEdge edgeSouth = new() { From = node.Id, To = nodeSouth.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.South }] };
        RawWorldGraphEdge edgeEast = new() { From = node.Id, To = nodeEast.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.East }] };
        RawWorldGraphEdge edgeWest = new() { From = node.Id, To = nodeWest.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.West }] };

        _worldDataProviderMock.Setup(p => p.GetEdgesFromNode(node.Id)).Returns([edgeNorth, edgeSouth, edgeEast, edgeWest]);
        _worldDataProviderMock.Setup(p => p.GetNode(nodeNorth.Id)).Returns(nodeNorth);
        _worldDataProviderMock.Setup(p => p.GetNode(nodeSouth.Id)).Returns(nodeSouth);
        _worldDataProviderMock.Setup(p => p.GetNode(nodeEast.Id)).Returns(nodeEast);
        _worldDataProviderMock.Setup(p => p.GetNode(nodeWest.Id)).Returns(nodeWest);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(nodeNorth)).Returns(mapNorth);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(nodeSouth)).Returns(mapSouth);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(nodeEast)).Returns(mapEast);
        _worldDataProviderMock.Setup(p => p.GetMapOfNode(nodeWest)).Returns(mapWest);

        IEnumerable<MapNodeWithPosition> nodes = _pathFinder.EnumerateNodesInDirection(node, Direction.North);

        nodes.Should()
            .BeEquivalentTo(
                [
                    new MapNodeWithPosition
                    {
                        NodeId = nodeNorth.Id,
                        MapId = mapNorth.MapId,
                        ZoneId = nodeNorth.ZoneId,
                        MapPosition = mapNorth.Position
                    }
                ]
            );
    }

    [TestMethod]
    public void ShouldNotFailIfMapNotFound()
    {
        RawWorldGraphNode node1 = FakeRawWorldGraphNode.Create();
        RawWorldGraphNode node2 = FakeRawWorldGraphNode.Create();
        RawWorldGraphEdge edge12 = new() { From = node1.Id, To = node2.Id, Transitions = [new RawWorldGraphEdgeTransition { Direction = RawWorldGraphEdgeDirection.East }] };

        _worldDataProviderMock.Setup(p => p.GetEdgesFromNode(node1.Id)).Returns([edge12]);
        _worldDataProviderMock.Setup(p => p.GetNode(node2.Id)).Returns(node2);

        IEnumerable<MapNodeWithPosition> nodes = _pathFinder.EnumerateNodesInDirection(node1, Direction.East);

        nodes.Should().BeEquivalentTo([new MapNodeWithPosition { NodeId = node2.Id, MapId = node2.MapId, ZoneId = node2.ZoneId, MapPosition = null }]);
    }
}
