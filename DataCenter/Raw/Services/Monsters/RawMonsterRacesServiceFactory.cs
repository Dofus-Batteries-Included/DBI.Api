using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Monsters;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonsterRacesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawMonsterRacesService, RawMonsterRace[]>(rawDataRepository, RawDataType.MonsterRaces, rawDataJsonOptionsProvider)
{
    protected override RawMonsterRacesService? CreateServiceImpl(RawMonsterRace[] data, CancellationToken cancellationToken) => new(data);
}
