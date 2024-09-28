namespace Server.Features.PathFinder.Controllers.Requests;

public class FindPathsRequest
{
    public int? FromCellNumber { get; set; }
    public int? ToCellNumber { get; set; }
}
