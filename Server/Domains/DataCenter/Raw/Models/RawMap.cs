namespace Server.Domains.DataCenter.Raw.Models;

public class RawMap
{
    public required Dictionary<int, RawCell> Cells { get; init; }
}
