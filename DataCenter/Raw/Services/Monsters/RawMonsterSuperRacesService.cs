using DBI.DataCenter.Raw.Models.Monsters;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonsterSuperRacesService(IReadOnlyCollection<RawMonsterSuperRace> monsterSuperRaces)
{
    readonly Dictionary<int, RawMonsterSuperRace> _monsterSuperRaces = monsterSuperRaces.ToDictionary(
        monsterSuperRace => monsterSuperRace.Id,
        monsterSuperRace => monsterSuperRace
    );

    public RawMonsterSuperRace? GetMonsterSuperRace(int monsterSuperRaceId) => _monsterSuperRaces.GetValueOrDefault(monsterSuperRaceId);
    public IEnumerable<RawMonsterSuperRace> GetMonsterSuperRaces() => _monsterSuperRaces.Values;
}
