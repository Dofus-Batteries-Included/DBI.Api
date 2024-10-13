namespace DBI.DataCenter.Raw.Models.Monsters;

public class RawMonsterRace
{
    public int Id { get; set; }
    public int SuperRaceId { get; set; }
    public int NameId { get; set; }
    public int AggressiveZoneSize { get; set; }
    public int AggressiveLevelDiff { get; set; }
    public string AggressiveImmunityCriterion { get; set; } = "";
    public int AggressiveAttackDelay { get; set; }
    public IReadOnlyList<uint> Monsters { get; set; } = [];
}
