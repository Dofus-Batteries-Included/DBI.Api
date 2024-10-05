using DBI.Server.Common.Models;

namespace DBI.Server.Features.DataCenter.Raw.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldMap
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public Position Origin { get; set; }
}
