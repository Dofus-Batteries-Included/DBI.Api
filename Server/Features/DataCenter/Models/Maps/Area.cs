using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

public class Area
{
    public required int? SuperAreaId { get; init; }
    public required long AreaId { get; init; }
    public required LocalizedText? Name { get; init; }
    public required Bounds Bounds { get; init; }
}
