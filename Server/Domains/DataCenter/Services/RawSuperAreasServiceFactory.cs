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
    public async Task<MapsService> CreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawMapsServiceFactory.CreateServiceAsync(version, cancellationToken),
            await rawMapPositionsServiceFactory.CreateServiceAsync(version, cancellationToken),
            await rawSubAreasServiceFactory.CreateServiceAsync(version, cancellationToken),
            await rawAreasServiceFactory.CreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.CreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesService(version, cancellationToken)
        );
}
