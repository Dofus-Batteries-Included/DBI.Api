namespace DBI.DataCenter.Raw.Models.Items;

public class RawItemSuperType
{
    public int Id { get; set; }
    public IReadOnlyList<int> PossiblePositions { get; set; } = [];
}
