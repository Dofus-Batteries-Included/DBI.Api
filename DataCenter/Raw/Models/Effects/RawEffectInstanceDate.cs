namespace DBI.DataCenter.Raw.Models.Effects;

public class RawEffectInstanceDate : RawEffectInstance
{
    public int Year { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
}
