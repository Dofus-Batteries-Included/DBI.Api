using Server.Domains.DataCenter.Raw.Services.I18N;
using Server.Domains.DataCenter.Raw.Services.Maps;

namespace Server.Domains.DataCenter.Services;

public class MapsServiceFactory(
    RawMapsServiceFactory rawMapsServiceFactory,
    RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
    RawSubAreasServiceFactory rawSubAreasServiceFactory,
    RawAreasServiceFactory rawAreasServiceFactory,
    RawSuperAreasServiceFactory rawSuperAreasServiceFactory,
    LanguagesServiceFactory languagesServiceFactory
)
{
    public async Task<MapsService> CreateService(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawMapsServiceFactory.CreateService(version, cancellationToken),
            await rawMapPositionsServiceFactory.CreateService(version, cancellationToken),
            await rawSubAreasServiceFactory.CreateService(version, cancellationToken),
            await rawAreasServiceFactory.CreateService(version, cancellationToken),
            await rawSuperAreasServiceFactory.CreateService(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesService(version, cancellationToken)
        );
}
