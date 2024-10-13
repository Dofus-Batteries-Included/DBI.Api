using DBI.DataCenter.Structured.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Items;

public class Item : ItemMinimal
{
    /// <summary>
    ///     The possible effects of the item.
    /// </summary>
    public IReadOnlyList<EffectInstance> PossibleEffects { get; set; } = [];

    /// <summary>
    ///     Is the item two-handed?
    /// </summary>
    public bool TwoHanded { get; set; }

    /// <summary>
    ///     Is the item usable?
    /// </summary>
    public bool Usable { get; set; }

    /// <summary>
    ///     Does the item needs user confirmation before use?
    /// </summary>
    public bool NeedUseConfirm { get; set; }

    /// <summary>
    ///     Is using the item on another character forbidden?
    /// </summary>
    public bool NonUsableOnAnother { get; set; }

    /// <summary>
    ///     Is the item useable by targeting another character or a cell?
    /// </summary>
    public bool Targetable { get; set; }

    /// <summary>
    ///     Is the item exchangeable?
    /// </summary>
    public bool Exchangeable { get; set; }

    /// <summary>
    ///     Is the item enhanceable?
    /// </summary>
    public bool Enhanceable { get; set; }

    /// <summary>
    ///     Is the item ethereal?
    /// </summary>
    public bool Ethereal { get; set; }

    /// <summary>
    ///     Is the item cursed?
    /// </summary>
    public bool Cursed { get; set; }

    /// <summary>
    ///     Is the item destructible by the user?
    /// </summary>
    public bool IsDestructible { get; set; }

    /// <summary>
    ///     Is the item legendary?
    /// </summary>
    public bool IsLegendary { get; set; }

    /// <summary>
    ///     Is the item colorable?
    /// </summary>
    public bool IsColorable { get; set; }

    /// <summary>
    ///     Is the item sealable?
    /// </summary>
    public bool IsSealable { get; set; }

    /// <summary>
    ///     Are the effects of the item hidden?
    /// </summary>
    public bool HideEffects { get; set; }

    /// <summary>
    ///     Is the bonus of the item secret?
    /// </summary>
    public bool BonusIsSecret { get; set; }

    /// <summary>
    ///     Is the item displayable on the official website?
    /// </summary>
    public bool ObjectIsDisplayOnWeb { get; set; }

    /// <summary>
    ///     The animation that the character plays when they use the item.
    /// </summary>
    public sbyte UseAnimationId { get; set; }

    /// <summary>
    ///     The visibility condition.
    /// </summary>
    public string Visibility { get; set; } = "";

    /// <summary>
    ///     The use criteria.
    /// </summary>
    public string Criteria { get; set; } = "";

    /// <summary>
    ///     The target of the use criteria.
    /// </summary>
    public string CriteriaTarget { get; set; } = "";

    /// <summary>
    ///     The appearance ID.
    /// </summary>
    public ushort AppearanceId { get; set; }

    /// <summary>
    ///     The important notice, if any.
    /// </summary>
    public string ImportantNotice { get; set; } = "";

    /// <summary>
    ///     The version of change.
    /// </summary>
    public string ChangeVersion { get; set; } = "";

    /// <summary>
    ///     The tooltip expiration date.
    /// </summary>
    public double TooltipExpirationDate { get; set; }
}
