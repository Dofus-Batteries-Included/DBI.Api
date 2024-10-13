using DBI.DataCenter.Raw.Models.Skills;

namespace DBI.DataCenter.Raw.Services.Skills;

/// <summary>
/// </summary>
public class RawSkillNamesService(IReadOnlyCollection<RawSkillName> skillNames)
{
    readonly Dictionary<int, RawSkillName> _skillNames = skillNames.ToDictionary(skillName => skillName.Id, skillName => skillName);

    public RawSkillName? GetSkillName(int skillNameId) => _skillNames.GetValueOrDefault(skillNameId);
    public IEnumerable<RawSkillName> GetSkillNames() => _skillNames.Values;
}
