using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Services;

namespace Server.Features.DataCenter.Raw.Services.WorldGraphs;

static class MapsServiceExtensions
{
    public static Map? GetMap(this MapsService service, RawWorldGraphNode node) => service.GetMap(node.MapId);
}
