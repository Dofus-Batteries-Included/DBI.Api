using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class SuperAreasService(RawSuperAreasService? rawSuperAreasService, LanguagesService languagesService)
{
    public IEnumerable<SuperArea>? GetSuperAreas() => rawSuperAreasService?.GetSuperAreas().Select(Cook);
    public IEnumerable<SuperArea>? GetSuperAreasInWorldMap(int worldMapId) => rawSuperAreasService?.GetSuperAreas().Where(a => a.WorldMapId == worldMapId).Select(Cook);

    public SuperArea? GetSuperArea(int superAreaId)
    {
        RawSuperArea? superArea = rawSuperAreasService?.GetSuperArea(superAreaId);
        return superArea == null ? null : Cook(superArea);
    }

    SuperArea Cook(RawSuperArea rawSuperArea) =>
        new()
        {
            SuperAreaId = rawSuperArea.Id,
            Name = languagesService?.Get(rawSuperArea.NameId)
        };
}
