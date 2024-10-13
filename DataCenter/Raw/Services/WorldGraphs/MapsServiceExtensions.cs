using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.DataCenter.Structured.Services.World;

namespace DBI.DataCenter.Raw.Services.WorldGraphs;

public static class MapsServiceExtensions
{
    public static Map? GetMap(this MapsService service, RawWorldGraphNode node) => service.GetMap(node.MapId);
}
