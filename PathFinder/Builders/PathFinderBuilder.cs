using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;

namespace DBI.PathFinder.Builders;

public class PathFinderBuilder
{
    public static PathFinderFromRawServicesBuilder FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) => new(rawWorldGraphService, mapsService);
}
