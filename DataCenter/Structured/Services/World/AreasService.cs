using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.DataCenter.Structured.Services.World;

/// <summary>
///     Get areas
/// </summary>
public class AreasService(
    RawWorldMapsService? rawWorldMapsService,
    RawSuperAreasService? rawSuperAreasService,
    RawAreasService? rawAreasService,
    LanguagesService? languagesService
)
{
    /// <summary>
    ///     Get all the areas in the game.
    /// </summary>
    public IEnumerable<Area>? GetAreas() => rawAreasService?.GetAreas().Select(Cook);

    /// <summary>
    ///     Get all the areas in the given world map.
    /// </summary>
    public IEnumerable<Area>? GetAreasInWorldMap(int worldMapId) => rawAreasService?.GetAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);

    /// <summary>
    ///     Get all the areas in the given super area.
    /// </summary>
    /// <param name="superAreaId"></param>
    /// <returns></returns>
    public IEnumerable<Area>? GetAreasInSuperArea(int? superAreaId) => rawAreasService?.GetAreas().Where(a => a.SuperAreaId == superAreaId).Select(Cook).ToArray();

    /// <summary>
    ///     Get the given area by id.
    /// </summary>
    public Area? GetArea(int areaId)
    {
        RawArea? area = rawAreasService?.GetArea(areaId);
        return area == null ? null : Cook(area);
    }

    Area Cook(RawArea area)
    {
        RawSuperArea? superArea = area.SuperAreaId is null ? null : rawSuperAreasService?.GetSuperArea(area.SuperAreaId.Value);

        int? worldMapId = area.WorldMapId ?? superArea?.WorldMapId;
        RawWorldMap? worldMap = worldMapId is null ? null : rawWorldMapsService?.GetWorldMap(worldMapId.Value);

        return new Area
        {
            WorldMapId = worldMapId,
            WorldMapName = worldMap is null ? null : languagesService?.Get(worldMap.NameId),
            SuperAreaId = area.SuperAreaId,
            SuperAreaName = superArea is null ? null : languagesService?.Get(superArea.NameId),
            AreaId = area.Id,
            Name = languagesService?.Get(area.NameId),
            Bounds = area.Bounds
        };
    }
}
