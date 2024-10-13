namespace DBI.DataCenter.Raw.Models.WorldGraphs;

/// <summary>
/// </summary>
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

    /// <summary>
    ///     The ID of the skill used to take the transition
    /// </summary>
    public int SkillId { get; set; }

    /// <summary>
    ///     The ID of the cell from which the user can take the transition
    /// </summary>
    public int CellId { get; set; }

    /// <summary>
    ///     The condition that the user must fulfill to be able to take the transition
    /// </summary>
    public string? Criterion { get; set; }
}
