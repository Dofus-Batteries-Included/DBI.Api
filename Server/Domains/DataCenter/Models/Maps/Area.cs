using Server.Common.Models;

namespace Server.Domains.DataCenter.Models.Maps;

public class Area
{
    public required long SuperAreaId { get; init; }
    public required long AreaId { get; init; }
    public required LocalizedText Name { get; init; }
    public required Bounds Bounds { get; init; }
}