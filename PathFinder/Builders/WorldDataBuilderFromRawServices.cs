using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services.World;
using DBI.PathFinder.DataProviders;

namespace DBI.PathFinder.Builders;

public class WorldDataBuilderFromRawServices(RawWorldGraphService rawWorldGraphService, RawMapsService rawMapsService, RawMapPositionsService rawMapPositionsService)
{
    public IWorldDataProvider Build() => new WorldDataFromRawServices(rawWorldGraphService, new MapsService(null, null, null, null, rawMapsService, rawMapPositionsService, null));
}
