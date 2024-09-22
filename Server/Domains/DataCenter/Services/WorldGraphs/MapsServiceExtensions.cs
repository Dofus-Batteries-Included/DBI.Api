using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Services.Maps;

namespace Server.Domains.DataCenter.Services.WorldGraphs;

public static class MapsServiceExtensions
{
    public static RawMapPosition? GetPositionOfNode(this MapsService service, WorldGraphNode node) => service.GetMap(node.MapId);
}
