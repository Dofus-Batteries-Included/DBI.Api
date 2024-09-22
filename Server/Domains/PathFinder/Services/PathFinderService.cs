using Server.Domains.DataCenter.Services.WorldGraphs;

namespace Server.Domains.PathFinder.Services;

public class PathFinderService
{
    readonly WorldGraphService _worldGraphService;

    public PathFinderService(WorldGraphService worldGraphService)
    {
        _worldGraphService = worldGraphService;
    }

    public void FindPath(long fromMapId, long toMapId)
    {

    }
}
