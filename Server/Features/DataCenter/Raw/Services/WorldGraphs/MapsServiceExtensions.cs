using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Models.WorldGraphs;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Raw.Services.WorldGraphs;

public static class MapsServiceExtensions
{
    public static Map? GetMap(this MapsService service, WorldGraphNode node) => service.GetMap(node.MapId);
}
