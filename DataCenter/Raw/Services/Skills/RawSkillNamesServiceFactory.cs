using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Skills;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Skills;

/// <summary>
/// </summary>
public class RawSkillNamesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawSkillNamesService, RawSkillName[]>(rawDataRepository, RawDataType.SkillNames, rawDataJsonOptionsProvider)
{
    protected override RawSkillNamesService? CreateServiceImpl(RawSkillName[] data, CancellationToken cancellationToken) => new(data);
}
