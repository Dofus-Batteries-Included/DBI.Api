using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceInteger : EffectInstance
{
    public EffectInstanceInteger() { }

    internal EffectInstanceInteger(RawEffectInstanceInteger instance)
    {
        Value = instance.Value;
    }

    public int Value { get; set; }
}
