using Server.Common.Models;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Maps;
using Server.Features.TreasureSolver.Models;
using Server.Features.TreasureSolver.Services.Clues;

namespace Server.Features.TreasureSolver.Services;

public class TreasureSolverService
{
    readonly FindCluesService _findCluesService;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;

    public TreasureSolverService(FindCluesService findCluesService, RawMapPositionsServiceFactory rawMapPositionsServiceFactory)
    {
        _findCluesService = findCluesService;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
    }

    public async Task<RawMapPosition?> FindNextMapAsync(RawMapPosition startMap, Direction direction, int clueId)
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

    async Task<RawMapPosition?> GuessTargetMapAsync(RawMapPosition startMap, Position position)
    {
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync();
        RawMapPosition[] maps = rawMapPositionsService.GetMapsAtPosition(position).ToArray();
        return maps.FirstOrDefault(m => m.WorldMap == startMap.WorldMap) ?? maps.FirstOrDefault();
    }
}
