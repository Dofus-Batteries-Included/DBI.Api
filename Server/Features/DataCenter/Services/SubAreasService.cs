using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class SubAreasService(RawAreasService? rawAreasService, RawSubAreasService? rawSubAreasService, LanguagesService languagesService)
{
    public IEnumerable<SubArea>? GetSubAreas() => rawSubAreasService?.GetSubAreas().Select(Cook);

    public IEnumerable<SubArea>? GetSubAreasInWorldMap(int worldMapId) => rawSubAreasService?.GetSubAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);

    public IEnumerable<SubArea>? GetSubAreasInSuperArea(int superAreaId)
    {
        HashSet<int>? areaIds = rawAreasService?.GetAreas().Where(a => a.SuperAreaId == superAreaId).Select(a => a.Id).ToHashSet();
        if (areaIds == null)
        {
            return [];
        }

        return rawSubAreasService?.GetSubAreas().Where(a => areaIds.Contains(a.AreaId)).Select(Cook);
    }

    public IEnumerable<SubArea>? GetSubAreasInArea(int areaId) => rawSubAreasService?.GetSubAreas().Where(a => a.AreaId == areaId).Select(Cook);

    public SubArea? GetSubArea(int subAreaId)
    {
        RawSubArea? subArea = rawSubAreasService?.GetSubArea(subAreaId);
        return subArea == null ? null : Cook(subArea);
    }

    SubArea Cook(RawSubArea subArea)
    {
        RawArea? area = rawAreasService?.GetArea(subArea.AreaId);

        return new SubArea
        {
            SuperAreaId = area?.SuperAreaId,
            AreaId = subArea.AreaId,
            SubAreaId = subArea.Id,
            Name = languagesService?.Get(subArea.NameId),
            Bounds = subArea.Bounds,
            Center = subArea.Center
        };
    }
}
