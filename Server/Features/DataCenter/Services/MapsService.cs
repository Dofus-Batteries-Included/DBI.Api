using Server.Common.Models;
using Server.Features.DataCenter.Models.Maps;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class MapsService(
    RawMapsService rawMapsService,
    RawMapPositionsService rawMapPositionsService,
    RawSubAreasService rawSubAreasService,
    RawAreasService rawAreasService,
    RawSuperAreasService rawSuperAreasService,
    RawWorldMapsService rawWorldMapService,
    LanguagesService languagesService
)
{
    public Map? GetMap(long mapId)
    {
        RawMap? rawMap = rawMapsService.GetMap(mapId);
        RawMapPosition? rawMapPosition = rawMapPositionsService.GetMap(mapId);
        if (rawMap == null || rawMapPosition == null)
        {
            return null;
        }

        RawSubArea? subArea = rawSubAreasService.GetSubArea(rawMapPosition.SubAreaId);
        RawArea? area = subArea is null ? null : rawAreasService.GetArea(subArea.AreaId);
        RawSuperArea? superArea = area?.SuperAreaId is null ? null : rawSuperAreasService.GetSuperArea(area.SuperAreaId.Value);

        int? worldMapId = subArea?.WorldMapId ?? area?.WorldMapId ?? superArea?.WorldMapId;
        RawWorldMap? worldMap = worldMapId is null ? null : rawWorldMapService.GetWorldMap(worldMapId.Value);

        return new Map
        {
            WorldMapId = worldMapId,
            WorldMapName = worldMap is null ? null : languagesService.Get(worldMap.NameId),
            SuperAreaId = superArea?.Id,
            SuperAreaName = superArea is null ? null : languagesService.Get(superArea.NameId),
            AreaId = area?.Id,
            AreaName = area is null ? null : languagesService.Get(area.NameId),
            SubAreaId = subArea?.Id,
            SubAreaName = subArea is null ? null : languagesService.Get(subArea.NameId),
            MapId = mapId,
            Name = languagesService.Get(rawMapPosition.NameId),
            Position = new Position(rawMapPosition.PosX, rawMapPosition.PosY),
            CellsCount = rawMap.Cells.Count
        };
    }

    public Cell? GetCell(long mapId, int cellNumber)
    {
        RawMap? rawMap = rawMapsService.GetMap(mapId);
        RawCell? cell = rawMap?.Cells.GetValueOrDefault(cellNumber);
        if (cell == null)
        {
            return null;
        }

        return new Cell
        {
            MapId = mapId,
            CellNumber = cell.CellNumber,
            Floor = cell.Floor,
            MoveZone = cell.MoveZone,
            LinkedZone = cell.LinkedZone,
            Speed = cell.Speed,
            Los = cell.Los,
            Visible = cell.Visible,
            NonWalkableDuringFight = cell.NonWalkableDuringFight,
            NonWalkableDuringRp = cell.NonWalkableDuringRp,
            HavenbagCell = cell.HavenbagCell
        };
    }
}
