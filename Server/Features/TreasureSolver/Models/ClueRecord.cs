namespace Server.Features.TreasureSolver.Models;

public class ClueRecord
{
    public DateTime RecordDate { get; set; }
    public long MapId { get; set; }
    public int ClueId { get; set; }
    public bool Found { get; set; }
}
