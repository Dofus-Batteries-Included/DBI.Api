using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemsService(IReadOnlyCollection<RawItem> items)
{
    readonly Dictionary<int, RawItem> _items = items.ToDictionary(item => (int)item.Id, item => item);

    public RawItem? GetItem(int itemId) => _items.GetValueOrDefault(itemId);
    public IEnumerable<RawItem> GetItems() => _items.Values;
}
