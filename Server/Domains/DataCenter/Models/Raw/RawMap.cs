namespace Server.Domains.DataCenter.Models.Raw;

public class RawMap
{
    public required Dictionary<int, RawCell> Cells { get; init; }
}
