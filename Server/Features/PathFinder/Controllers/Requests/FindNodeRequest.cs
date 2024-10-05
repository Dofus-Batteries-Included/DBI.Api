using System.Text.Json.Serialization;
using DBI.Server.Common.Models;

namespace DBI.Server.Features.PathFinder.Controllers.Requests;

/// <summary>
///     Base class for requests to find a node.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "search")]
[JsonDerivedType(typeof(FindNodeById), "by-id")]
[JsonDerivedType(typeof(FindNodeByMap), "by-map")]
[JsonDerivedType(typeof(FindNodeAtPosition), "at-position")]
public abstract class FindNodeRequest
{
}

/// <summary>
///     Search for a node by its unique ID.
/// </summary>
public class FindNodeById : FindNodeRequest
{
    /// <summary>
    ///     The unique ID of the node.
    /// </summary>
    public long NodeId { get; set; }
}

/// <summary>
///     Search for a node in a map.
/// </summary>
public class FindNodeByMap : FindNodeRequest
{
    /// <summary>
    ///     The unique ID of the node.
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     The number of the cell that should be linked to the node.
    /// </summary>
    /// <remarks>
    ///     If cell number is provided, their will be at most one node in the result.
    /// </remarks>
    public int? CellNumber { get; set; }
}

/// <summary>
///     Search for a node at a given position.
/// </summary>
/// <remarks>
///     This is the least precise way to look for a node. There might be multiple maps at the given position so even with a cell number there might be multiple nodes.
/// </remarks>
public class FindNodeAtPosition : FindNodeRequest
{
    /// <summary>
    ///     The position at which the node should be.
    /// </summary>
    public Position Position { get; set; }

    /// <summary>
    ///     The number of the cell that should be linked to the node.
    /// </summary>
    public int? CellNumber { get; set; }
}
