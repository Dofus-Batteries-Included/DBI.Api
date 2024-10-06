using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Models;
using DBI.PathFinder.Strategies;
using FluentAssertions;
using Moq;
using Test.FakeData;
using Path = DBI.PathFinder.Models.Path;

namespace Test.PathFinder;

[TestClass]
public class PathFinder_GetShortestPath
{
    Mock<IPathFindingStrategy> _pathFindingStrategyMock = null!;
    Mock<IWorldDataProvider> _worldDataProviderMock = null!;
    DBI.PathFinder.PathFinder _pathFinder = null!;

    [TestInitialize]
    public void Initialize()
    {
        _pathFindingStrategyMock = new Mock<IPathFindingStrategy>();
        _worldDataProviderMock = new Mock<IWorldDataProvider>();
        _pathFinder = new DBI.PathFinder.PathFinder(_pathFindingStrategyMock.Object, _worldDataProviderMock.Object);
    }

    [TestMethod]
    public void ShouldGetShortestPath()
    {
        Map fromMap = FakeMap.Create();
        Map toMap = FakeMap.Create();

        RawWorldGraphNode fromNode = FakeRawWorldGraphNode.Create(fromMap);
        RawWorldGraphNode toNode = FakeRawWorldGraphNode.Create(toMap);

        Map pathMap1 = FakeMap.Create();
        Map pathMap2 = FakeMap.Create();

        RawWorldGraphNode pathNode1 = FakeRawWorldGraphNode.Create(pathMap1);
        RawWorldGraphNode pathNode2 = FakeRawWorldGraphNode.Create(pathMap2);
        RawWorldGraphNode[] path = [fromNode, pathNode1, pathNode2, toNode];

        RawWorldGraphEdge edge1 = new()
        {
            From = fromNode.Id, To = pathNode1.Id,
            Transitions = [new RawWorldGraphEdgeTransition { Type = RawWorldGraphEdgeType.ScrollAction, Direction = RawWorldGraphEdgeDirection.East }]
        };
        RawWorldGraphEdge edge12 = new()
        {
            From = pathNode1.Id, To = pathNode2.Id,
            Transitions = [new RawWorldGraphEdgeTransition { Type = RawWorldGraphEdgeType.ScrollAction, Direction = RawWorldGraphEdgeDirection.South }]
        };
        RawWorldGraphEdge edge2 = new()
        {
            From = pathNode2.Id, To = toNode.Id,
            Transitions = [new RawWorldGraphEdgeTransition { Type = RawWorldGraphEdgeType.ScrollAction, Direction = RawWorldGraphEdgeDirection.West }]
        };

        _worldDataProviderMock.SetupNodeAndMap(fromNode, fromMap);
        _worldDataProviderMock.SetupNodeAndMap(toNode, toMap);
        _worldDataProviderMock.SetupNodeAndMap(pathNode1, pathMap1);
        _worldDataProviderMock.SetupNodeAndMap(pathNode2, pathMap2);
        _worldDataProviderMock.SetupEdges(edge1, edge12, edge2);

        _pathFindingStrategyMock.Setup(s => s.ComputePath(fromNode, toNode)).Returns(path);

        Path? result = _pathFinder.GetShortestPath(fromNode, toNode);

        _pathFindingStrategyMock.Verify(s => s.ComputePath(fromNode, toNode));

        result.Should().NotBeNull();
        result.Should()
            .BeEquivalentTo(
                new Path
                {
                    From = new MapNodeWithPosition
                    {
                        NodeId = fromNode.Id,
                        MapId = fromNode.MapId,
                        ZoneId = fromNode.ZoneId,
                        MapPosition = fromMap.Position
                    },
                    To = new MapNodeWithPosition
                    {
                        NodeId = toNode.Id,
                        MapId = toNode.MapId,
                        ZoneId = toNode.ZoneId,
                        MapPosition = toMap.Position
                    },
                    Steps =
                    [
                        new PathStep
                        {
                            Node = new MapNodeWithPosition
                            {
                                NodeId = pathNode1.Id,
                                MapId = pathNode1.MapId,
                                ZoneId = pathNode1.ZoneId,
                                MapPosition = pathMap1.Position
                            },
                            Transition = new MapScrollTransitionMinimal
                            {
                                Direction = Direction.East
                            }
                        },
                        new PathStep
                        {
                            Node = new MapNodeWithPosition
                            {
                                NodeId = fromNode.Id,
                                MapId = fromNode.MapId,
                                ZoneId = fromNode.ZoneId,
                                MapPosition = fromMap.Position
                            },
                            Transition = new MapScrollTransitionMinimal
                            {
                                Direction = Direction.South
                            }
                        },
                        new PathStep
                        {
                            Node = new MapNodeWithPosition
                            {
                                NodeId = pathNode2.Id,
                                MapId = pathNode2.MapId,
                                ZoneId = pathNode2.ZoneId,
                                MapPosition = pathMap2.Position
                            },
                            Transition = new MapScrollTransitionMinimal
                            {
                                Direction = Direction.West
                            }
                        }
                    ]
                }
            );
    }
}
