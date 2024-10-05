using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Raw.Models;
using DBI.Server.Features.DataCenter.Raw.Services.I18N;
using DBI.Server.Features.DataCenter.Raw.Services.Maps;

namespace DBI.Server.Features.DataCenter.Services;

/// <summary>
///     Get sub areas.
/// </summary>
public class SubAreasService(
    RawWorldMapsService? rawWorldMapsService,
    RawSuperAreasService? rawSuperAreasService,
    RawAreasService? rawAreasService,
    RawSubAreasService? rawSubAreasService,
    LanguagesService languagesService
)
{
    /// <summary>
    ///     Get all the sub areas in the game.
    /// </summary>
    public IEnumerable<SubArea>? GetSubAreas() => rawSubAreasService?.GetSubAreas().Select(Cook);

    /// <summary>
    ///     Get all the sub areas in the given world map.
    /// </summary>
    public IEnumerable<SubArea>? GetSubAreasInWorldMap(int worldMapId) => rawSubAreasService?.GetSubAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);

    /// <summary>
    ///     Get all the sub areas in the given super area.
    /// </summary>
    public IEnumerable<SubArea>? GetSubAreasInSuperArea(int superAreaId)
    {
        HashSet<int>? areaIds = rawAreasService?.GetAreas().Where(a => a.SuperAreaId == superAreaId).Select(a => a.Id).ToHashSet();
        if (areaIds == null)
        {
            return [];
        }

        return rawSubAreasService?.GetSubAreas().Where(a => areaIds.Contains(a.AreaId)).Select(Cook);
    }

    /// <summary>
    ///     Get all the sub areas in the given area.
    /// </summary>
    /// <param name="areaId"></param>
    /// <returns></returns>
    public IEnumerable<SubArea>? GetSubAreasInArea(int areaId) => rawSubAreasService?.GetSubAreas().Where(a => a.AreaId == areaId).Select(Cook);

    /// <summary>
    ///     Get the given sub area by id.
    /// </summary>
    public SubArea? GetSubArea(int subAreaId)
    {
        RawSubArea? subArea = rawSubAreasService?.GetSubArea(subAreaId);
        return subArea == null ? null : Cook(subArea);
    }

    SubArea Cook(RawSubArea subArea)
    {
        RawArea? area = rawAreasService?.GetArea(subArea.AreaId);
        RawSuperArea? superArea = area?.SuperAreaId is null ? null : rawSuperAreasService?.GetSuperArea(area.SuperAreaId.Value);
        RawWorldMap? worldMap = rawWorldMapsService?.GetWorldMap(subArea.WorldMapId);

        return new SubArea
        {
            WorldMapId = subArea.WorldMapId,
            WorldMapName = worldMap is null ? null : languagesService?.Get(worldMap.NameId),
            SuperAreaId = superArea?.Id,
            SuperAreaName = superArea is null ? null : languagesService?.Get(superArea.NameId),
            AreaId = subArea.AreaId,
            AreaName = area is null ? null : languagesService?.Get(area.NameId),
            SubAreaId = subArea.Id,
            Name = languagesService?.Get(subArea.NameId),
            Bounds = subArea.Bounds,
            Center = subArea.Center
        };
    }
}
