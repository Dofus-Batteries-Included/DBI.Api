namespace DBI.DataCenter.Raw.Models;

/// <summary>
///     2D position.
/// </summary>
/// <param name="X">The horizontal coordinate.</param>
/// <param name="Y">The vertical coordinate.</param>
public record struct RawPosition(int X, int Y);

public static class RawPositionExtensions
{
    public static int DistanceTo(this RawPosition from, RawPosition to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
