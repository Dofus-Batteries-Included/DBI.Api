namespace Server.Features.DataCenter.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     Type of <c>Core.PathFinding.WorldPathfinding.Transition.m_type</c>
/// </summary>
public enum WorldGraphEdgeType
{
    Unspecified = 0,
    Scroll = 1,
    ScrollAction = 2,
    MapEvent = 4,
    MapAction = 8,
    MapObstacle = 16, // 0x00000010
    Interactive = 32, // 0x00000020
    NpcAction = 64 // 0x00000040
}
