using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.DataCenter.Structured.Models.Items;

public class ItemRecyclingData
{
    public float RecyclingNuggets { get; set; }
    public IReadOnlyList<SubArea> FavoriteRecyclingSubAreas { get; set; } = [];
    public IReadOnlyList<IReadOnlyList<Item>> ResourcesBySubArea { get; set; } = [];
}
