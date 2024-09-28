namespace Server.Common.Models;

/// <summary>
///     2D position.
/// </summary>
/// <param name="X">The horizontal coordinate.</param>
/// <param name="Y">The vertical coordinate.</param>
public record struct Position(int X, int Y);

static class PositionExtensions
{
    public static Position MoveInDirection(this Position start, Direction direction, int distance = 1) =>
        direction switch
        {
            Direction.West => start with { X = start.X - distance },
            Direction.East => start with { X = start.X + distance },
            Direction.North => start with { Y = start.Y - distance },
            Direction.South => start with { Y = start.Y + distance },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public static int DistanceTo(this Position from, Position to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
