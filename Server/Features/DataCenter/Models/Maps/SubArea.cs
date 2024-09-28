using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

public class SubArea
{
    public required long SuperAreaId { get; init; }
    public required long AreaId { get; init; }
    public required long SubAreaId { get; init; }
    public required LocalizedText Name { get; init; }
    public required Position Center { get; init; }
    public required Bounds Bounds { get; init; }
}
