using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Items;
using DBI.DataCenter.Structured.Models.Items;
using DBI.DataCenter.Structured.Services.World;

namespace DBI.DataCenter.Structured.Services.Items;

public class ItemsRecyclingDataService(RawItemsService? rawItemsService, SubAreasService? subAreasService, ItemsService itemsService)
{
    public ItemRecyclingData? GetItemRecyclingData(int itemId)
    {
        RawItem? rawItem = rawItemsService?.GetItem(itemId);
        if (rawItem == null)
        {
            return null;
        }

        return new ItemRecyclingData
        {
            RecyclingNuggets = rawItem.RecyclingNuggets,
            FavoriteRecyclingSubAreas = subAreasService == null
                ? []
                : rawItem.FavoriteRecyclingSubAreas.Select(subAreasService.GetSubArea).Where(a => a != null).Select(a => a!).ToArray(),
            ResourcesBySubArea = rawItem.ResourcesBySubarea.Select(r => r.Select(itemsService.GetItem).Where(i => i != null).Select(i => i!).ToArray()).ToArray()
        };
    }
}
