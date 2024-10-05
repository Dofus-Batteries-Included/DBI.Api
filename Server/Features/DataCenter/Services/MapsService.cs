using DBI.Server.Common.Models;
using DBI.Server.Features.DataCenter.Models.Maps;
using DBI.Server.Features.DataCenter.Raw.Models;
using DBI.Server.Features.DataCenter.Raw.Services.I18N;
using DBI.Server.Features.DataCenter.Raw.Services.Maps;
using RawArea = DBI.Server.Features.DataCenter.Raw.Models.RawArea;
using RawCell = DBI.Server.Features.DataCenter.Raw.Models.RawCell;
using RawSubArea = DBI.Server.Features.DataCenter.Raw.Models.RawSubArea;
using RawSuperArea = DBI.Server.Features.DataCenter.Raw.Models.RawSuperArea;
using RawWorldMap = DBI.Server.Features.DataCenter.Raw.Models.RawWorldMap;

namespace DBI.Server.Features.DataCenter.Services;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class MapsService(
    RawWorldMapsService? rawWorldMapsService,
    RawSuperAreasService? rawSuperAreasService,
    RawAreasService? rawAreasService,
    RawSubAreasService? rawSubAreasService,
    RawMapsService? rawMapsService,
    RawMapPositionsService? rawMapPositionsService,
    LanguagesService languagesService
)
{
    public IEnumerable<Map> GetMaps() => GetMapsImpl().Select(x => Cook(x.RawMapPosition, x.RawMap));

    public IEnumerable<Map> GetMapsInWorldMap(int worldMapId) => GetMapsImpl().Where(x => x.RawMapPosition.WorldMap == worldMapId).Select(x => Cook(x.RawMapPosition, x.RawMap));

    public IEnumerable<Map> GetMapsInSuperArea(int superAreaId)
    {
        HashSet<int>? areaIds = rawAreasService?.GetAreas().Where(a => a.SuperAreaId == superAreaId).Select(a => a.Id).ToHashSet();
        if (areaIds == null)
        {
            return [];
        }

        HashSet<int>? subAreaIds = rawSubAreasService?.GetSubAreas().Where(a => areaIds.Contains(a.AreaId)).Select(a => a.Id).ToHashSet();
        if (subAreaIds == null)
        {
            return [];
        }

        return GetMapsImpl().Where(x => subAreaIds.Contains(x.RawMapPosition.SubAreaId)).Select(x => Cook(x.RawMapPosition, x.RawMap));
    }

    public IEnumerable<Map> GetMapsInArea(int areaId)
    {
        HashSet<int>? subAreaIds = rawSubAreasService?.GetSubAreas().Where(a => a.AreaId == areaId).Select(a => a.Id).ToHashSet();
        if (subAreaIds == null)
        {
            return [];
        }

        return GetMapsImpl().Where(x => subAreaIds.Contains(x.RawMapPosition.SubAreaId)).Select(x => Cook(x.RawMapPosition, x.RawMap));
    }

    public IEnumerable<Map> GetMapsInSubArea(int subAreaId) => GetMapsImpl().Where(x => x.RawMapPosition.SubAreaId == subAreaId).Select(x => Cook(x.RawMapPosition, x.RawMap));

    public Map? GetMap(long mapId)
    {
        Raw.Models.RawMap? rawMap = rawMapsService?.GetMap(mapId);
        Raw.Models.RawMapPosition? rawMapPosition = rawMapPositionsService?.GetMap(mapId);
        return rawMap == null || rawMapPosition == null ? null : Cook(rawMapPosition, rawMap);
    }

    public IEnumerable<MapCell>? GetCells(long mapId)
    {
        Raw.Models.RawMap? rawMap = rawMapsService?.GetMap(mapId);
        return rawMap?.Cells.Values.Select(c => Cook(mapId, c));
    }

    public MapCell? GetCell(long mapId, int cellNumber)
    {
        Raw.Models.RawMap? rawMap = rawMapsService?.GetMap(mapId);
        RawCell? cell = rawMap?.Cells.GetValueOrDefault(cellNumber);
        return cell == null ? null : Cook(mapId, cell);
    }

    Map Cook(Raw.Models.RawMapPosition rawMapPosition, Raw.Models.RawMap rawMap)
    {
        RawSubArea? subArea = rawSubAreasService?.GetSubArea(rawMapPosition.SubAreaId);
        RawArea? area = subArea is null ? null : rawAreasService?.GetArea(subArea.AreaId);
        RawSuperArea? superArea = area?.SuperAreaId is null ? null : rawSuperAreasService?.GetSuperArea(area.SuperAreaId.Value);
        RawWorldMap? worldMap = rawWorldMapsService?.GetWorldMap(((int?)rawMapPosition.WorldMap).Value);

        return new Map
        {
            WorldMapId = rawMapPosition.WorldMap,
            WorldMapName = worldMap is null ? null : languagesService?.Get(worldMap.NameId),
            SuperAreaId = superArea?.Id,
            SuperAreaName = superArea is null ? null : languagesService?.Get(superArea.NameId),
            AreaId = area?.Id,
            AreaName = area is null ? null : languagesService?.Get(area.NameId),
            SubAreaId = rawMapPosition.SubAreaId,
            SubAreaName = subArea is null ? null : languagesService?.Get(subArea.NameId),
            MapId = rawMapPosition.MapId,
            MapName = languagesService?.Get(rawMapPosition.NameId),
            Position = new Position(rawMapPosition.PosX, rawMapPosition.PosY),
            CellsCount = rawMap.Cells.Count
        };
    }

    static MapCell Cook(long mapId, RawCell cell) =>
        new()
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

    IEnumerable<(Raw.Models.RawMapPosition RawMapPosition, Raw.Models.RawMap RawMap)> GetMapsImpl()
    {
        if (rawMapsService == null || rawMapPositionsService == null)
        {
            yield break;
        }

        foreach (RawMapPosition rawMapPosition in rawMapPositionsService.GetMaps())
        {
            RawMap? rawMap = rawMapsService.GetMap(rawMapPosition.MapId);
            if (rawMap != null)
            {
                yield return (rawMapPosition, rawMap);
            }
        }
    }
}
