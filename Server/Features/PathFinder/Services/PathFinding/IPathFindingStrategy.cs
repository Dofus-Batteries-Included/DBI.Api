using DBI.DataCenter.Raw.Models.WorldGraphs;

namespace DBI.Server.Features.PathFinder.Services.PathFinding;

interface IPathFindingStrategy
{
    IReadOnlyList<RawWorldGraphNode>? ComputePath(RawWorldGraphNode from, RawWorldGraphNode to);
}
