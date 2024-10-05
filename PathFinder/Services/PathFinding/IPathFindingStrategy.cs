using DBI.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.PathFinder.Services.PathFinding;

public interface IPathFindingStrategy
{
    IReadOnlyList<RawWorldGraphNode>? ComputePath(RawWorldGraphNode from, RawWorldGraphNode to);
}
