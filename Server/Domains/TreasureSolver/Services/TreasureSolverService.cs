using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues;
using Server.Domains.TreasureSolver.Services.Maps;

namespace Server.Domains.TreasureSolver.Services;

public class TreasureSolverService
{
    readonly FindCluesService _findCluesService;
    readonly IMapsService _mapsService;

    public TreasureSolverService(FindCluesService findCluesService, IMapsService mapsService)
    {
        _findCluesService = findCluesService;
        _mapsService = mapsService;
    }

    public async Task<Map?> FindNextMapAsync(Map startMap, Direction direction, int clueId)
    {
        Position position = new(startMap.PosX, startMap.PosY);
        for (int i = 0; i < 10; i++)
        {
            position = position.MoveInDirection(direction);
            IReadOnlyCollection<Clue> clues = await _findCluesService.FindCluesAtPositionAsync(position.X, position.Y);
            if (clues.Any(c => c.ClueId == clueId))
            {
                return GuessTargetMap(startMap, position);
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

    Map? GuessTargetMap(Map startMap, Position position)
    {
        Map[] maps = _mapsService.GetMapsAtPosition(position).ToArray();
        return maps.FirstOrDefault(m => m.WorldMap == startMap.WorldMap) ?? maps.FirstOrDefault();
    }
}
