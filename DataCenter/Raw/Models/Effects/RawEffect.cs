namespace DBI.DataCenter.Raw.Models.Effects;

public class RawEffect
{
    public int Id { get; set; }
    public int IconId { get; set; }
    public bool Active { get; set; }
    public bool Boost { get; set; }
    public int DescriptionId { get; set; }
    public string TheoreticalDescriptionId { get; set; } = string.Empty;
    public int ElementId { get; set; }
    public int OppositeId { get; set; }
    public int BonusType { get; set; }
    public int Category { get; set; }
    public int Characteristic { get; set; }
    public string CharacteristicOperator { get; set; } = string.Empty;
    public int TheoreticalPattern { get; set; }
    public bool UseDice { get; set; }
    public bool UseInFight { get; set; }
    public int EffectPriority { get; set; }
    public float EffectPowerRate { get; set; }
    public int EffectTriggerDuration { get; set; }
    public bool ForceMinMax { get; set; }
    public bool IsInPercent { get; set; }
    public bool ShowInSet { get; set; }
    public bool ShowInTooltip { get; set; }
    public bool HideValueInTooltip { get; set; }
    public int TextIconReferenceId { get; set; }
}
