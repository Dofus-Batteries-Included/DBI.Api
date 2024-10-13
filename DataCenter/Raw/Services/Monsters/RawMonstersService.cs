using DBI.DataCenter.Raw.Models.Monsters;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonstersService(IReadOnlyCollection<RawMonster> monsters)
{
    readonly Dictionary<int, RawMonster> _monsters = monsters.ToDictionary(monster => monster.Id, monster => monster);

    public RawMonster? GetMonster(int monsterId) => _monsters.GetValueOrDefault(monsterId);
    public IEnumerable<RawMonster> GetMonsters() => _monsters.Values;
}
