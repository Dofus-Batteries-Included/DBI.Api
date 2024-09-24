namespace Server.Domains.DataCenter.Models.Maps;

public class Cell
{
    public required long MapId { get; init; }
    public required int CellNumber { get; init; }
    public required int Floor { get; init; }
    public required int MoveZone { get; init; }
    public required int LinkedZone { get; init; }
    public required int Speed { get; init; }
    public required bool Los { get; init; }
    public required bool Visible { get; init; }
    public required bool NonWalkableDuringFight { get; init; }
    public required bool NonWalkableDuringRp { get; init; }
    public required bool HavenbagCell { get; init; }
}
