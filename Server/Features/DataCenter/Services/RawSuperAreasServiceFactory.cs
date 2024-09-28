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
            await rawMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawMapPositionsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSubAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesService(version, cancellationToken)
        );
}
