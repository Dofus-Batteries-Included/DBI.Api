using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;

namespace DBI.PathFinder.Builders;

public static class WorldDataBuilder
{
    public static WorldDataBuilderFromRawServices FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) => new(rawWorldGraphService, mapsService);
    public static WorldDataBuilderFromDdcGithubRepository FromDdcGithubRepository() => new();
}
