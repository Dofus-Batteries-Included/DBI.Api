namespace DBI.DataCenter.Raw.Models;

/// <summary>
/// </summary>
public class RawMap
{
    public required Dictionary<int, RawCell> Cells { get; init; }
}
