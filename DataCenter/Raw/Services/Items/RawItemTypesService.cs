using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemTypesService(IReadOnlyCollection<RawItemType> itemTypes)
{
    readonly Dictionary<int, RawItemType> _itemTypes = itemTypes.ToDictionary(itemType => itemType.Id, itemType => itemType);

    public RawItemType? GetItemType(int itemTypeId) => _itemTypes.GetValueOrDefault(itemTypeId);
    public IEnumerable<RawItemType> GetItemTypes() => _itemTypes.Values;
}
