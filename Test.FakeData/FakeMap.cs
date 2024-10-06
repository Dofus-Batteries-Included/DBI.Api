using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Structured.Models.Maps;

namespace Test.FakeData;

public static class FakeMap
{
    public static Map Create()
    {
        int random = Random.Shared.Next();

        return new Map
        {
            WorldMapId = random + 1,
            WorldMapName = FakeLocalizedText.Create($"WORLD_MAP_{random}"),
            SuperAreaId = random + 2,
            SuperAreaName = FakeLocalizedText.Create($"SUPER_AREA_{random}"),
            AreaId = random + 3,
            AreaName = FakeLocalizedText.Create($"AREA_{random}"),
            SubAreaId = random + 4,
            SubAreaName = FakeLocalizedText.Create($"SUB_AREA_{random}"),
            MapId = random,
            MapName = FakeLocalizedText.Create($"MAP_{random}"),
            Position = new RawPosition(random + 6, random + 7),
            CellsCount = random + 8
        };
    }
}
