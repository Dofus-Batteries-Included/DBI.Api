using System.Text.Json.Serialization;
using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A transition between two nodes
/// </summary>
[JsonDerivedType(typeof(MapScrollTransition), "scroll")]
public class MapTransition
{
    /// <summary>
    ///     The start node.
    /// </summary>
    public required MapNode From { get; set; }

    /// <summary>
    ///     The end node.
    /// </summary>
    public required MapNode To { get; set; }
}

/// <summary>
///     A scroll transition between two nodes
/// </summary>
public class MapScrollTransition : MapTransition
{
    /// <summary>
    ///     The direction of the scroll between the start and end nodes.
    /// </summary>
    public Direction Direction { get; set; }
}
