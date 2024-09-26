using Server.Common.Models;

namespace Server.Domains.DataCenter.Models.Maps;

public class SuperArea
{
    public required long SuperAreaId { get; init; }
    public required LocalizedText Name { get; init; }
}