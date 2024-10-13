using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceString : EffectInstance
{
    public EffectInstanceString() { }

    internal EffectInstanceString(RawEffectInstanceString instance)
    {
        Text = instance.Text;
    }

    public string Text { get; set; } = string.Empty;
}
