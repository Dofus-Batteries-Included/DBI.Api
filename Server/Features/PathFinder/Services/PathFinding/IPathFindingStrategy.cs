using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Features.PathFinder.Services.PathFinding;

interface IPathFindingStrategy
{
    IReadOnlyList<RawWorldGraphNode>? ComputePath(RawWorldGraphNode from, RawWorldGraphNode to);
}
