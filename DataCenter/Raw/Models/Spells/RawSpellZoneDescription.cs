using DBI.DataCenter.Raw.Models.Enums;

namespace DBI.DataCenter.Raw.Models.Spells;

public class RawSpellZoneDescription
{
    public IReadOnlyList<int> CellIds { get; set; } = [];
    public RawSpellZoneShape Shape { get; set; }
    public byte Param1 { get; set; }
    public byte Param2 { get; set; }
    public sbyte DamageDecreaseStepPercent { get; set; }
    public sbyte MaxDamageDecreaseApplyCount { get; set; }
    public bool IsStopAtTarget { get; set; }
}
