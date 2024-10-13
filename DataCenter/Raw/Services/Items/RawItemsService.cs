using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemsService(IReadOnlyCollection<RawItem> items)
{
    readonly Dictionary<ushort, RawItem> _items = items.ToDictionary(item => item.Id, item => item);

    public RawItem? GetItem(ushort itemId) => _items.GetValueOrDefault(itemId);
    public IEnumerable<RawItem> GetItems() => _items.Values;
}
