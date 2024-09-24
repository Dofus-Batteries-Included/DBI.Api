using Server.Common.Models;

namespace Server.Domains.DataCenter.Raw.Models;

public class RawArea
{
    public int Id { get; init; }
    public int NameId { get; init; }
    public int? WorldMapId { get; init; }
    public int? SuperAreaId { get; init; }
    public Bounds Bounds { get; init; }
    public bool ContainHouses { get; init; }
    public bool ContainPaddocks { get; init; }
}
