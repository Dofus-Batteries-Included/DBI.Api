using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class AreasService(RawAreasService? rawAreasService, LanguagesService languagesService)
{
    public IEnumerable<Area>? GetAreas() => rawAreasService?.GetAreas().Select(Cook);
    public IEnumerable<Area>? GetAreasInWorldMap(int worldMapId) => rawAreasService?.GetAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);
    public IEnumerable<Area>? GetAreasInSuperArea(int? superAreaId) => rawAreasService?.GetAreas().Where(a => a.SuperAreaId == superAreaId).Select(Cook).ToArray();

    public Area? GetArea(int areaId)
    {
        RawArea? area = rawAreasService?.GetArea(areaId);
        return area == null ? null : Cook(area);
    }

    Area Cook(RawArea area) =>
        new()
        {
            SuperAreaId = area.SuperAreaId,
            AreaId = area.Id,
            Name = languagesService?.Get(area.NameId),
            Bounds = area.Bounds
        };
}
