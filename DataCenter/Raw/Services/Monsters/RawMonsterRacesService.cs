using DBI.DataCenter.Raw.Models.Monsters;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonsterRacesService(IReadOnlyCollection<RawMonsterRace> monsterRaces)
{
    readonly Dictionary<int, RawMonsterRace> _monsterRaces = monsterRaces.ToDictionary(monsterRace => monsterRace.Id, monsterRace => monsterRace);

    public RawMonsterRace? GetMonsterRace(int monsterRaceId) => _monsterRaces.GetValueOrDefault(monsterRaceId);
    public IEnumerable<RawMonsterRace> GetMonsterRaces() => _monsterRaces.Values;
}
