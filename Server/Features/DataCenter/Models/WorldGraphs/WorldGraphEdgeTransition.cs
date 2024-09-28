namespace Server.Features.DataCenter.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class WorldGraphEdgeTransition
{
    /// <summary>
    ///     The type of transition
    /// </summary>
    public WorldGraphEdgeType? Type { get; set; }

    /// <summary>
    ///     The direction of the transition
    /// </summary>
    public WorldGraphEdgeDirection? Direction { get; set; }

    /// <summary>
    ///     The ID of the map
    /// </summary>
    public long MapId { get; set; }
}
