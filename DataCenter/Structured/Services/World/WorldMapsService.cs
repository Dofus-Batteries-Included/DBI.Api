using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.DataCenter.Structured.Services.World;

/// <summary>
///     Get world maps.
/// </summary>
public class WorldMapsService(RawWorldMapsService? rawWorldMapService, LanguagesService languagesService)
{
    /// <summary>
    ///     Get all the world maps in the game.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<WorldMap>? GetWorldMaps() => rawWorldMapService?.GetWorldMaps().Select(Cook);

    /// <summary>
    ///     Get the given world map by id.
    /// </summary>
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
