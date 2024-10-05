namespace DBI.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraphNode : IEquatable<RawWorldGraphNode>
{
    /// <summary>
    ///     The unique ID of the node
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    ///     The ID of the underlying map.
    /// </summary>
    /// <remarks>
    ///     The ID of the map is only unique in a given zone.
    /// </remarks>
    public long MapId { get; init; }

    /// <summary>
    ///     The zone of the underlying map
    /// </summary>
    public int ZoneId { get; init; }

    public bool Equals(RawWorldGraphNode? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj.GetType() != GetType())
        {
            return false;
        }
        return Equals((RawWorldGraphNode)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(RawWorldGraphNode? left, RawWorldGraphNode? right) => Equals(left, right);

    public static bool operator !=(RawWorldGraphNode? left, RawWorldGraphNode? right) => !Equals(left, right);
}
