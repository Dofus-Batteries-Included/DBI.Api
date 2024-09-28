using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class WorldMapsService(RawWorldMapsService? rawWorldMapService, LanguagesService languagesService)
{
    public IEnumerable<WorldMap>? GetWorldMaps() => rawWorldMapService?.GetWorldMaps().Select(Cook);

    public WorldMap? GetWorldMap(int worldMapId)
    {
        RawWorldMap? worldMap = rawWorldMapService?.GetWorldMap(worldMapId);
        return worldMap == null ? null : Cook(worldMap);
    }

    WorldMap Cook(RawWorldMap worldMap) =>
        new()
        {
            WorldMapId = worldMap.Id,
            Name = languagesService?.Get(worldMap.NameId),
            Origin = worldMap.Origin
        };
}
