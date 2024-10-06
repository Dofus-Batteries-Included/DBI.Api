using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;

namespace DBI.PathFinder.Builders;

public class WorldDataBuilderFromRawServices
{
    readonly RawWorldGraphService _rawWorldGraphService;
    readonly MapsService _mapsService;

    public WorldDataBuilderFromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService)
    {
        _rawWorldGraphService = rawWorldGraphService;
        _mapsService = mapsService;
    }

    public IWorldDataProvider Build() => new WorldDataFromRawServices(_rawWorldGraphService, _mapsService);
}
