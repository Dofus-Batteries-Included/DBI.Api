using Server.Common.Models;

namespace Server.Features.DataCenter.Raw.Models;

public class RawWorldMap
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public Position Origin { get; set; }
}
