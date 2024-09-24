using Server.Domains.DataCenter.Models.Maps;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Services;

namespace Server.Domains.DataCenter.Raw.Services.WorldGraphs;

public static class MapsServiceExtensions
{
    public static Map? GetMap(this MapsService service, WorldGraphNode node) => service.GetMap(node.MapId);
}
