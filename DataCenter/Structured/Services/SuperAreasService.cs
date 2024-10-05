using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.DataCenter.Structured.Services;

/// <summary>
///     Get super areas
/// </summary>
public class SuperAreasService(RawWorldMapsService? rawWorldMapsService, RawSuperAreasService? rawSuperAreasService, LanguagesService languagesService)
{
    /// <summary>
    ///     Get all the super areas in the game.
    /// </summary>
    public IEnumerable<SuperArea>? GetSuperAreas() => rawSuperAreasService?.GetSuperAreas().Select(Cook);

    /// <summary>
    ///     Get all the super areas in the given world map.
    /// </summary>
    public IEnumerable<SuperArea>? GetSuperAreasInWorldMap(int worldMapId) => rawSuperAreasService?.GetSuperAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);

    /// <summary>
    ///     Get the given super area by id.
    /// </summary>
    /// <param name="superAreaId"></param>
    /// <returns></returns>
    public SuperArea? GetSuperArea(int superAreaId)
    {
        RawSuperArea? superArea = rawSuperAreasService?.GetSuperArea(superAreaId);
        return superArea == null ? null : Cook(superArea);
    }

    SuperArea Cook(RawSuperArea rawSuperArea)
    {
        RawWorldMap? worldMap = rawSuperArea.WorldMapId is null ? null : rawWorldMapsService?.GetWorldMap(rawSuperArea.WorldMapId.Value);

        return new SuperArea
        {
            WorldMapId = rawSuperArea.WorldMapId,
            WorldMapName = worldMap is null ? null : languagesService?.Get(worldMap.NameId),
            SuperAreaId = rawSuperArea.Id,
            Name = languagesService?.Get(rawSuperArea.NameId)
        };
    }
}
