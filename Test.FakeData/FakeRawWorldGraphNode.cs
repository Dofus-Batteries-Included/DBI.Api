using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Structured.Models.Maps;

namespace Test.FakeData;

public static class FakeRawWorldGraphNode
{
    public static RawWorldGraphNode Create()
    {
        int random = Random.Shared.Next();

        return new RawWorldGraphNode
        {
            Id = random,
            MapId = random + 1,
            ZoneId = random + 2
        };
    }

    public static RawWorldGraphNode Create(Map map)
    {
        int random = Random.Shared.Next();

        return new RawWorldGraphNode
        {
            Id = random,
            MapId = map.MapId,
            ZoneId = random + 2
        };
    }
}
