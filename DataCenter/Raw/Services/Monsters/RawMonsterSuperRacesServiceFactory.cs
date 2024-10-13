using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Monsters;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonsterSuperRacesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawMonsterSuperRacesService, RawMonsterSuperRace[]>(rawDataRepository, RawDataType.MonsterSuperRaces, rawDataJsonOptionsProvider)
{
    protected override RawMonsterSuperRacesService? CreateServiceImpl(RawMonsterSuperRace[] data, CancellationToken cancellationToken) => new(data);
}
