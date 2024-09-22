using Server.Common.Models;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues;

namespace Server.Domains.TreasureSolver.Services;

public class TreasureSolverService
{
    readonly FindCluesService _findCluesService;
    readonly MapsServiceFactory _mapsServiceFactory;

    public TreasureSolverService(FindCluesService findCluesService, MapsServiceFactory mapsServiceFactory)
    {
        _findCluesService = findCluesService;
        _mapsServiceFactory = mapsServiceFactory;
    }

    public async Task<MapPositions?> FindNextMapAsync(MapPositions startMap, Direction direction, int clueId)
    {
        Position position = new(startMap.PosX, startMap.PosY);
        for (int i = 0; i < 10; i++)
        {
            position = position.MoveInDirection(direction);
            IReadOnlyCollection<Clue> clues = await _findCluesService.FindCluesAtPositionAsync(position.X, position.Y);
            if (clues.Any(c => c.ClueId == clueId))
            {
                return await GuessTargetMapAsync(startMap, position);
            }
        }

        return null;
    }

    public async Task<Position?> FindNextMapAsync(Position startPosition, Direction direction, int clueId)
    {
        Position position = startPosition;
        for (int i = 0; i < 10; i++)
        {
            position = position.MoveInDirection(direction);
            IReadOnlyCollection<Clue> clues = await _findCluesService.FindCluesAtPositionAsync(position.X, position.Y);
            if (clues.Any(c => c.ClueId == clueId))
            {
                return position;
            }
        }

        return null;
    }

    async Task<MapPositions?> GuessTargetMapAsync(MapPositions startMap, Position position)
    {
        MapsService mapsService = await _mapsServiceFactory.CreateService();
        MapPositions[] maps = mapsService.GetMapsAtPosition(position).ToArray();
        return maps.FirstOrDefault(m => m.WorldMap == startMap.WorldMap) ?? maps.FirstOrDefault();
    }
}
