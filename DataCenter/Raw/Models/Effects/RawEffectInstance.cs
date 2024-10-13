using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models.Enums;
using DBI.DataCenter.Raw.Models.Spells;

namespace DBI.DataCenter.Raw.Models.Effects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RawEffectInstanceString), "string")]
[JsonDerivedType(typeof(RawEffectInstanceDice), "dice")]
[JsonDerivedType(typeof(RawEffectInstanceInteger), "integer")]
[JsonDerivedType(typeof(RawEffectInstanceMinMax), "min-max")]
[JsonDerivedType(typeof(RawEffectInstanceDate), "date")]
[JsonDerivedType(typeof(RawEffectInstanceDuration), "duration")]
[JsonDerivedType(typeof(RawEffectInstanceLadder), "ladder")]
[JsonDerivedType(typeof(RawEffectInstanceMount), "mount")]
[JsonDerivedType(typeof(RawEffectInstanceCreature), "creature")]
public class RawEffectInstance
{
    public RawActionId EffectId { get; set; }
    public int EffectUid { get; set; }
    public short BaseEffectId { get; set; }
    public sbyte EffectElement { get; set; }
    public byte Delay { get; set; }
    public sbyte Duration { get; set; }
    public byte Dispellable { get; set; }
    public short Group { get; set; }
    public short Modificator { get; set; }
    public int Order { get; set; }
    public bool Trigger { get; set; }
    public string Triggers { get; set; } = string.Empty;
    public bool ShowInSet { get; set; }
    public float Random { get; set; }
    public short SpellId { get; set; }
    public short TargetId { get; set; }
    public string TargetMask { get; set; } = string.Empty;
    public bool ZoneStopAtTarget { get; set; }
    public RawSpellZoneDescription? ZoneDescription { get; set; }
    public RawSpellZoneShape ZoneShape { get; set; }
    public byte ZoneSize { get; set; }
    public byte ZoneMinSize { get; set; }
    public int ZoneDamageDecreaseStepPercent { get; set; }
    public int ZoneMaxDamageDecreaseApplyCount { get; set; }
    public bool VisibleInTooltip { get; set; }
    public bool VisibleOnTerrain { get; set; }
    public bool VisibleInBuffUi { get; set; }
    public bool VisibleInFightLog { get; set; }
    public bool ForClientOnly { get; set; }
}
