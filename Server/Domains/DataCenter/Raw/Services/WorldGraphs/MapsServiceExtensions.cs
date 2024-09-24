using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Maps;

namespace Server.Domains.DataCenter.Raw.Services.WorldGraphs;

public static class MapsServiceExtensions
{
    public static RawMapPosition? GetPositionOfNode(this RawMapPositionsService service, WorldGraphNode node) => service.GetMap(node.MapId);
}
