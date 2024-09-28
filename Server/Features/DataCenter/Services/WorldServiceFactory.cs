using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;

namespace Server.Features.DataCenter.Services;

public class WorldServiceFactory(
    RawMapsServiceFactory rawMapsServiceFactory,
    RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
    RawSubAreasServiceFactory rawSubAreasServiceFactory,
    RawAreasServiceFactory rawAreasServiceFactory,
    RawSuperAreasServiceFactory rawSuperAreasServiceFactory,
    RawWorldMapsServiceFactory rawWorldMapsServiceFactory,
    LanguagesServiceFactory languagesServiceFactory
)
{
    public async Task<WorldMapsService> CreateWorldMapsServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    public async Task<SuperAreasService> CreateSuperAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    public async Task<AreasService> CreateAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken), await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken));

    public async Task<SubAreasService> CreateSubAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSubAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    public async Task<MapsService> CreateMapsServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSubAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawMapPositionsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );
}
