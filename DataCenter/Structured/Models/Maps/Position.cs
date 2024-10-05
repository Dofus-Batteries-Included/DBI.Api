using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Maps;

namespace DBI.DataCenter.Structured.Models.Maps;

/// <summary>
///     2D position.
/// </summary>
/// <param name="X">The horizontal coordinate.</param>
/// <param name="Y">The vertical coordinate.</param>
public record struct Position(int X, int Y);

public static class PositionExtensions
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
    public static int DistanceTo(this RawPosition from, Position to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
    public static int DistanceTo(this Position from, RawPosition to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}

public static class RawMapPositionsServiceExtensions
{
    public static IEnumerable<RawMapPosition> GetMapsAtPosition(this RawMapPositionsService service, Position position) =>
        service.GetMapsAtPosition(new RawPosition(position.X, position.Y));
}