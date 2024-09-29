namespace Server.Features.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraphEdgeTransition
{
    /// <summary>
    ///     The type of transition
    /// </summary>
    public RawWorldGraphEdgeType? Type { get; set; }

    /// <summary>
    ///     The direction of the transition
    /// </summary>
    public RawWorldGraphEdgeDirection? Direction { get; set; }

    /// <summary>
    ///     The ID of the map
    /// </summary>
    public long MapId { get; set; }
}
