namespace DBI.Server.Features.DataCenter.Raw.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawMap
{
    public required Dictionary<int, RawCell> Cells { get; init; }
}
