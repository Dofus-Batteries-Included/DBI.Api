namespace Server.Features.PathFinder.Controllers.Requests;

/// <summary>
///     Additional constraints to apply while searching for paths.
/// </summary>
public class FindPathsRequest
{
    /// <summary>
    ///     The cell of the source map from which the path should start.
    ///     Providing the cell number helps to reduce the number of nodes to consider when a map contains multiple nodes.
    /// </summary>
    public int? FromCellNumber { get; set; }

    /// <summary>
    ///     The cell of the destination map to which the path should lead.
    ///     Providing the cell number helps to reduce the number of nodes to consider when a map contains multiple nodes.
    /// </summary>
    public int? ToCellNumber { get; set; }
}
