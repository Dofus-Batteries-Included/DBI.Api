using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Common.Models;

/// <summary>
///     2D direction.
/// </summary>
public enum Direction
{
    /// <summary>
    ///     Up, towards negative Y values.
    /// </summary>
    North,

    /// <summary>
    ///     Down, towards positive Y values.
    /// </summary>
    South,

    /// <summary>
    ///     Left, towards negative X values.
    /// </summary>
    East,

    /// <summary>
    ///     Right, towards positive X values.
    /// </summary>
    West
}

static class DirectionMappingExtensions
{
    public static Direction Cook(this RawWorldGraphEdgeDirection? direction)
    {
        switch (direction)
        {
            case RawWorldGraphEdgeDirection.North:
                return Direction.North;
            case RawWorldGraphEdgeDirection.East:
                return Direction.East;
            case RawWorldGraphEdgeDirection.South:
                return Direction.South;
            case RawWorldGraphEdgeDirection.West:
                return Direction.West;
            case null:
            case RawWorldGraphEdgeDirection.Random:
            case RawWorldGraphEdgeDirection.Same:
            case RawWorldGraphEdgeDirection.Opposite:
            case RawWorldGraphEdgeDirection.Invalid:
            case RawWorldGraphEdgeDirection.SouthEast:
            case RawWorldGraphEdgeDirection.SouthWest:
            case RawWorldGraphEdgeDirection.NorthWest:
            case RawWorldGraphEdgeDirection.NorthEast:
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
