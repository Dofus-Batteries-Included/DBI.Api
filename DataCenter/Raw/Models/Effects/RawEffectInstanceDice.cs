namespace DBI.DataCenter.Raw.Models.Effects;

public class RawEffectInstanceDice : RawEffectInstanceInteger
{
    public int DiceNum { get; set; }
    public int DiceSide { get; set; }
    public bool DisplayZero { get; set; }
}
