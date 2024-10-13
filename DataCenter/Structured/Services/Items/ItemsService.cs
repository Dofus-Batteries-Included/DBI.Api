using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Items;
using DBI.DataCenter.Structured.Models.Effects;
using DBI.DataCenter.Structured.Models.Items;

namespace DBI.DataCenter.Structured.Services.Items;

public class ItemsService(RawItemTypesService? rawItemTypesService, RawItemsService? rawItemsService, RawItemSetsService? rawItemSetsService, LanguagesService? languagesService)
{
    public IEnumerable<Item> GetItems() => rawItemsService?.GetItems().Select(Cook) ?? [];
    public IEnumerable<Item> GetItemsInType(int itemTypeId) => rawItemsService?.GetItems().Where(i => i.ItemTypeId == itemTypeId).Select(Cook) ?? [];

    public IEnumerable<Item> GetItemsInSuperType(ItemSuperType superType)
    {
        HashSet<int>? itemTypes = rawItemTypesService?.GetItemTypes().Where(t => t.SuperTypeId == (int)superType).Select(t => t.Id).ToHashSet();
        if (itemTypes == null)
        {
            return [];
        }

        return rawItemsService?.GetItems().Where(i => itemTypes.Contains(i.ItemTypeId)).Select(Cook) ?? [];
    }

    public Item? GetItem(int itemId)
    {
        RawItem? item = rawItemsService?.GetItem(itemId);
        return item == null ? null : Cook(item);
    }

    Item Cook(RawItem item)
    {
        RawItemType? itemType = rawItemTypesService?.GetItemType(item.ItemTypeId);
        RawItemSet? itemSet = rawItemSetsService?.GetItemSet(item.ItemSetId);

        return new Item
        {
            SuperType = itemType?.SuperTypeId == null ? ItemSuperType.None : (ItemSuperType)itemType.SuperTypeId,
            ItemTypeId = item.ItemTypeId,
            ItemTypeName = itemType == null ? null : languagesService?.Get(itemType.NameId),
            Id = item.Id,
            Level = item.Level,
            IconId = item.IconId,
            Name = languagesService?.Get(item.NameId),
            Description = languagesService?.Get(item.DescriptionId),
            PossibleEffects = item.PossibleEffects.Select(e => e.Cook()).ToArray(),
            Price = item.Price,
            Weight = item.Weight,
            ItemSetId = item.ItemSetId,
            ItemSetName = itemSet == null ? null : languagesService?.Get(itemSet.NameId),
            TwoHanded = item.TwoHanded,
            Usable = item.Usable,
            NeedUseConfirm = item.NeedUseConfirm,
            NonUsableOnAnother = item.NonUsableOnAnother,
            Targetable = item.Targetable,
            Exchangeable = item.Exchangeable,
            Enhanceable = item.Enhanceable,
            Ethereal = item.Ethereal,
            Cursed = item.Cursed,
            IsDestructible = item.IsDestructible,
            IsLegendary = item.IsLegendary,
            IsColorable = item.IsColorable,
            IsSealable = item.IsSealable,
            HideEffects = item.HideEffects,
            BonusIsSecret = item.BonusIsSecret,
            ObjectIsDisplayOnWeb = item.ObjectIsDisplayOnWeb,
            UseAnimationId = item.UseAnimationId,
            Visibility = item.Visibility,
            Criteria = item.Criteria,
            CriteriaTarget = item.CriteriaTarget,
            AppearanceId = item.AppearanceId,
            ImportantNotice = item.ImportantNoticeId,
            ChangeVersion = item.ChangeVersion,
            TooltipExpirationDate = item.TooltipExpirationDate
        };
    }
}
