namespace DBI.DataCenter.Raw.Models.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraphEdge
{
    /// <summary>
    ///     The ID of the source node
    /// </summary>
    public long From { get; set; }

    /// <summary>
    ///     The ID of the target node
    /// </summary>
    public long To { get; set; }

    public IReadOnlyCollection<RawWorldGraphEdgeTransition>? Transitions { get; set; }
}
