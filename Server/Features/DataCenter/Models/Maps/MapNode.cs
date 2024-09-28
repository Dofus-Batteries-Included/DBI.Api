namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A node in a map.
/// </summary>
public class MapNode
{
    /// <summary>
    ///     The unique ID of the node in the world graph.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     The unique ID of the containing map.
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     The ID of the zone in the map.
    ///     Multiple nodes in the same map are identified by their zone ID.
    /// </summary>
    public int ZoneId { get; set; }
}
