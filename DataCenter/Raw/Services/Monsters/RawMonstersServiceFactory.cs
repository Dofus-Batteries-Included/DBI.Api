using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Monsters;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Monsters;

/// <summary>
/// </summary>
public class RawMonstersServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawMonstersService, RawMonster[]>(rawDataRepository, RawDataType.Monsters, rawDataJsonOptionsProvider)
{
    protected override RawMonstersService? CreateServiceImpl(RawMonster[] data, CancellationToken cancellationToken) => new(data);
}
