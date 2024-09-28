namespace Server.Features.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///     Type of <c>Core.PathFinding.WorldPathfinding.Transition.m_direction</c>
/// </summary>
public enum WorldGraphEdgeDirection
{
    Random = -4, // 0xFFFFFFFC
    Same = -3, // 0xFFFFFFFD
    Opposite = -2, // 0xFFFFFFFE
    Invalid = -1, // 0xFFFFFFFF
    East = 0,
    SouthEast = 1,
    South = 2,
    SouthWest = 3,
    West = 4,
    NorthWest = 5,
    North = 6,
    NorthEast = 7
}
