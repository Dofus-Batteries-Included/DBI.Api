namespace DBI.DataCenter.Raw.Models;

/// <summary>\n/// </summary>
public class RawArea
{
    public int Id { get; init; }
    public int NameId { get; init; }
    public int? WorldMapId { get; init; }
    public int? SuperAreaId { get; init; }
    public RawBounds Bounds { get; init; } = new();
    public bool ContainHouses { get; init; }
    public bool ContainPaddocks { get; init; }
}
