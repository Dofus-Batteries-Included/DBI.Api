using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models.Effects;
using DBI.DataCenter.Raw.Models.Spells;
using DBI.DataCenter.Structured.Models.Enums;

namespace DBI.DataCenter.Structured.Models.Effects;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(EffectInstanceString), "string")]
[JsonDerivedType(typeof(EffectInstanceDice), "dice")]
[JsonDerivedType(typeof(EffectInstanceInteger), "integer")]
[JsonDerivedType(typeof(EffectInstanceMinMax), "min-max")]
[JsonDerivedType(typeof(EffectInstanceDate), "date")]
[JsonDerivedType(typeof(EffectInstanceDuration), "duration")]
[JsonDerivedType(typeof(EffectInstanceLadder), "ladder")]
[JsonDerivedType(typeof(EffectInstanceMount), "mount")]
[JsonDerivedType(typeof(EffectInstanceCreature), "creature")]
public class EffectInstance
{
    public EffectInstance() { }

    internal EffectInstance(RawEffectInstance instance)
    {
        EffectId = instance.EffectId.Cook();
        EffectUid = instance.EffectUid;
        BaseEffectId = instance.BaseEffectId;
        EffectElement = instance.EffectElement;
        Delay = instance.Delay;
        Duration = instance.Duration;
        Dispellable = instance.Dispellable;
        Group = instance.Group;
        Modificator = instance.Modificator;
        Order = instance.Order;
        Trigger = instance.Trigger;
        Triggers = instance.Triggers;
        ShowInSet = instance.ShowInSet;
        Random = instance.Random;
        SpellId = instance.SpellId;
        TargetId = instance.TargetId;
        TargetMask = instance.TargetMask;
        ZoneStopAtTarget = instance.ZoneStopAtTarget;
        ZoneDescription = instance.ZoneDescription;
        ZoneShape = instance.ZoneShape.Cook();
        ZoneSize = instance.ZoneSize;
        ZoneMinSize = instance.ZoneMinSize;
        ZoneDamageDecreaseStepPercent = instance.ZoneDamageDecreaseStepPercent;
        ZoneMaxDamageDecreaseApplyCount = instance.ZoneMaxDamageDecreaseApplyCount;
        VisibleInTooltip = instance.VisibleInTooltip;
        VisibleOnTerrain = instance.VisibleOnTerrain;
        VisibleInBuffUi = instance.VisibleInBuffUi;
        VisibleInFightLog = instance.VisibleInFightLog;
        ForClientOnly = instance.ForClientOnly;
    }

    public ActionId EffectId { get; set; }
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
    public SpellZoneShape ZoneShape { get; set; }
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

static class EffectInstanceMappingExtensions
{
    public static EffectInstance Cook(this RawEffectInstance instance) =>
        instance switch
        {
            RawEffectInstanceLadder ladder => new EffectInstanceLadder(ladder),
            RawEffectInstanceCreature creature => new EffectInstanceCreature(creature),
            RawEffectInstanceDate date => new EffectInstanceDate(date),
            RawEffectInstanceDice dice => new EffectInstanceDice(dice),
            RawEffectInstanceDuration duration => new EffectInstanceDuration(duration),
            RawEffectInstanceInteger integer => new EffectInstanceInteger(integer),
            RawEffectInstanceMinMax minMax => new EffectInstanceMinMax(minMax),
            RawEffectInstanceMount mount => new EffectInstanceMount(mount),
            RawEffectInstanceString str => new EffectInstanceString(str),
            _ => throw new ArgumentOutOfRangeException(nameof(instance))
        };
}
