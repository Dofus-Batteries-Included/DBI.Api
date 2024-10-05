using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Raw.Models.WorldGraphs;
using DBI.Server.Features.DataCenter.Services;

namespace DBI.Server.Features.DataCenter.Raw.Services.WorldGraphs;

static class MapsServiceExtensions
{
    public static Map? GetMap(this MapsService service, RawWorldGraphNode node) => service.GetMap(node.MapId);
}
