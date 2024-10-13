﻿using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Raw.Models.Items;

public class RawItem
{
    public ushort Id { get; set; }
    public byte Level { get; set; }
    public int NameId { get; set; }
    public int DescriptionId { get; set; }
    public int ItemTypeId { get; set; }
    public IReadOnlyList<RawEffectInstance> PossibleEffects { get; set; } = [];
    public float Price { get; set; }
    public uint Weight { get; set; }
    public float RecyclingNuggets { get; set; }
    public IReadOnlyList<ushort> RecipeIds { get; set; } = [];
    public byte RecipeSlots { get; set; }
    public bool SecretRecipe { get; set; }
    public short ItemSetId { get; set; }
    public bool TwoHanded { get; set; }
    public bool Usable { get; set; }
    public bool NeedUseConfirm { get; set; }
    public bool NonUsableOnAnother { get; set; }
    public bool Targetable { get; set; }
    public bool Exchangeable { get; set; }
    public bool Enhanceable { get; set; }
    public bool Ethereal { get; set; }
    public bool Cursed { get; set; }
    public bool IsDestructible { get; set; }
    public bool IsLegendary { get; set; }
    public bool IsColorable { get; set; }
    public bool IsSealable { get; set; }
    public bool HideEffects { get; set; }
    public bool BonusIsSecret { get; set; }
    public bool ObjectIsDisplayOnWeb { get; set; }
}
