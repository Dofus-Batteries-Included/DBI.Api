namespace DBI.DataCenter.Raw.Models.Monsters;

public class RawMonster
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public ushort GfxId { get; set; }
    public short Race { get; set; }
    public IReadOnlyList<RawMonsterGrade> Grades { get; set; } = [];
    public string Look { get; set; } = "";
    public IReadOnlyList<RawMonsterDrop> Drops { get; set; } = [];
    public IReadOnlyList<RawMonsterDrop> TemporisDrops { get; set; } = [];
    public IReadOnlyList<uint> Subareas { get; set; } = [];
    public IReadOnlyList<int> Spells { get; set; } = [];
    public IReadOnlyList<string> SpellGrades { get; set; } = [];
    public ushort FavoriteSubareaId { get; set; }
    public ushort CorrespondingMiniBossId { get; set; }
    public sbyte SpeedAdjust { get; set; }
    public sbyte CreatureBoneId { get; set; }
    public IReadOnlyList<uint> IncompatibleIdols { get; set; } = [];
    public IReadOnlyList<uint> IncompatibleChallenges { get; set; } = [];
    public byte AggressiveZoneSize { get; set; }
    public short AggressiveLevelDiff { get; set; }
    public string AggressiveImmunityCriterion { get; set; } = "";
    public short AggressiveAttackDelay { get; set; }
    public byte ScaleGradeRef { get; set; }
    public IReadOnlyList<IReadOnlyList<float>> CharacRatios { get; set; } = [];
}
