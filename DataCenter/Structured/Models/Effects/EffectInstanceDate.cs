using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceDate : EffectInstance
{
    public EffectInstanceDate() { }

    internal EffectInstanceDate(RawEffectInstanceDate instance)
    {
        Year = instance.Year;
        Day = instance.Day;
        Month = instance.Month;
        Hour = instance.Hour;
        Minute = instance.Minute;
    }

    public int Year { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
}
