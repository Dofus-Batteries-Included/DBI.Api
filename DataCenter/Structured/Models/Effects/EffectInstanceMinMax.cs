using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceMinMax : EffectInstance
{
    public EffectInstanceMinMax() { }

    internal EffectInstanceMinMax(RawEffectInstanceMinMax instance)
    {
        Min = instance.Min;
        Max = instance.Max;
    }

    public int Min { get; set; }
    public int Max { get; set; }
}
