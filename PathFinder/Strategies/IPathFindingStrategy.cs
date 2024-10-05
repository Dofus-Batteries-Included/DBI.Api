using DBI.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.PathFinder.Strategies;

public interface IPathFindingStrategy
{
    IReadOnlyList<RawWorldGraphNode>? ComputePath(RawWorldGraphNode from, RawWorldGraphNode to);
}
