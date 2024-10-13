using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceDice : EffectInstanceInteger
{
    public EffectInstanceDice() { }

    internal EffectInstanceDice(RawEffectInstanceDice instance)
    {
        DiceNum = instance.DiceNum;
        DiceSide = instance.DiceSide;
        DisplayZero = instance.DisplayZero;
    }

    public int DiceNum { get; set; }
    public int DiceSide { get; set; }
    public bool DisplayZero { get; set; }
}
