using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceDuration : EffectInstance
{
    public EffectInstanceDuration() { }

    internal EffectInstanceDuration(RawEffectInstanceDuration instance)
    {
        Days = instance.Days;
        Hours = instance.Hours;
        Minutes = instance.Minutes;
    }

    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
}
