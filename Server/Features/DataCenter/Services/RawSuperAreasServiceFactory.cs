using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class MapsServiceFactory(
    RawMapsServiceFactory rawMapsServiceFactory,
    RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
    RawSubAreasServiceFactory rawSubAreasServiceFactory,
    RawAreasServiceFactory rawAreasServiceFactory,
    RawSuperAreasServiceFactory rawSuperAreasServiceFactory,
    RawWorldMapsServiceFactory rawWorldMapsServiceFactory,
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
            await rawWorldMapsServiceFactory.CreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesService(version, cancellationToken)
        );
}
