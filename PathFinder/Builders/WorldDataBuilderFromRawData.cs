using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.PathFinder.DataProviders;

namespace DBI.PathFinder.Builders;

public class WorldDataBuilderFromRawData(RawWorldGraph rawWorldGraph, Dictionary<long, RawMap> maps, IReadOnlyCollection<RawMapPosition> mapPositions)
{
    public IWorldDataProvider Build() =>
        WorldDataBuilder.FromRawServices(new RawWorldGraphService(rawWorldGraph), new RawMapsService(maps), new RawMapPositionsService(mapPositions)).Build();
}
