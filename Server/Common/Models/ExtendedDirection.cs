using Server.Features.DataCenter.Raw.Models.WorldGraphs;

namespace Server.Common.Models;

/// <summary>
///     2D direction.
/// </summary>
public enum ExtendedDirection
{
    /// <summary>
    ///     Random direction, will be decided by the server.
    /// </summary>
    Random = -4,

    /// <summary>
    ///     Same direction.
    /// </summary>
    Same = -3,

    /// <summary>
    ///     Opposite direction.
    /// </summary>
    Opposite = -2,

    /// <summary>
    ///     Invalid direction.
    /// </summary>
    Invalid = -1,

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
    West,

    /// <summary>
    ///     Up right, towards positive X values and negative Y values.
    /// </summary>
    NorthEast,

    /// <summary>
    ///     Up left, towards negative X values and negative Y values.
    /// </summary>
    NorthWest,

    /// <summary>
    ///     Down right, towards positive X values and positive Y values.
    /// </summary>
    SouthEast,

    /// <summary>
    ///     Down left, towards negative X values and positive Y values.
    /// </summary>
    SouthWest
}

static class ExtendedDirectionMappingExtensions
{
    public static ExtendedDirection CookExtendedDirection(this RawWorldGraphEdgeDirection direction)
    {
        switch (direction)
        {
            case RawWorldGraphEdgeDirection.Random:
                return ExtendedDirection.Random;
            case RawWorldGraphEdgeDirection.Same:
                return ExtendedDirection.Same;
            case RawWorldGraphEdgeDirection.Opposite:
                return ExtendedDirection.Opposite;
            case RawWorldGraphEdgeDirection.Invalid:
                return ExtendedDirection.Invalid;
            case RawWorldGraphEdgeDirection.North:
                return ExtendedDirection.North;
            case RawWorldGraphEdgeDirection.East:
                return ExtendedDirection.East;
            case RawWorldGraphEdgeDirection.South:
                return ExtendedDirection.South;
            case RawWorldGraphEdgeDirection.West:
                return ExtendedDirection.West;
            case RawWorldGraphEdgeDirection.SouthEast:
                return ExtendedDirection.SouthEast;
            case RawWorldGraphEdgeDirection.SouthWest:
                return ExtendedDirection.SouthWest;
            case RawWorldGraphEdgeDirection.NorthWest:
                return ExtendedDirection.NorthWest;
            case RawWorldGraphEdgeDirection.NorthEast:
                return ExtendedDirection.NorthEast;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
