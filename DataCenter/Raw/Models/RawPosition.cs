namespace DBI.DataCenter.Raw.Models;

/// <summary>
///     2D position.
/// </summary>
/// <param name="X">The horizontal coordinate.</param>
/// <param name="Y">The vertical coordinate.</param>
public readonly struct RawPosition : IEquatable<RawPosition>
{
    public RawPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public bool Equals(RawPosition other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is RawPosition other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(RawPosition left, RawPosition right) => left.Equals(right);

    public static bool operator !=(RawPosition left, RawPosition right) => !left.Equals(right);
}

public static class RawPositionExtensions
{
    public static int DistanceTo(this RawPosition from, RawPosition to) => Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}
