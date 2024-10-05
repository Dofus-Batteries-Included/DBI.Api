using DBI.Server.Features.DataCenter.Raw.Services.I18N;
using DBI.Server.Features.DataCenter.Raw.Services.Maps;
using DBI.Server.Features.DataCenter.Raw.Services.WorldGraphs;

namespace DBI.Server.Features.DataCenter.Services;

/// <summary>
///     Create instances of services for world data.
/// </summary>
public class WorldServiceFactory(
    RawMapsServiceFactory rawMapsServiceFactory,
    RawMapPositionsServiceFactory rawMapPositionsServiceFactory,
    RawSubAreasServiceFactory rawSubAreasServiceFactory,
    RawAreasServiceFactory rawAreasServiceFactory,
    RawSuperAreasServiceFactory rawSuperAreasServiceFactory,
    RawWorldMapsServiceFactory rawWorldMapsServiceFactory,
    RawWorldGraphServiceFactory rawRawWorldGraphServiceFactory,
    LanguagesServiceFactory languagesServiceFactory
)
{
    /// <summary>
    ///     Create an instance of WorldMapsService for the given version of the game.
    /// </summary>
    public async Task<WorldMapsService> CreateWorldMapsServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    /// <summary>
    ///     Create an instance of SuperAreasService for the given version of the game.
    /// </summary>
    public async Task<SuperAreasService> CreateSuperAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    /// <summary>
    ///     Create an instance of AreasService for the given version of the game.
    /// </summary>
    public async Task<AreasService> CreateAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    /// <summary>
    ///     Create an instance of SubAreasService for the given version of the game.
    /// </summary>
    public async Task<SubAreasService> CreateSubAreasServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawWorldMapsServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSuperAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await rawSubAreasServiceFactory.TryCreateServiceAsync(version, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(version, cancellationToken)
        );

    /// <summary>
    ///     Create an instance of MapsService for the given version of the game.
    /// </summary>
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

    /// <summary>
    ///     Create an instance of MapsService for the given version of the game.
    /// </summary>
    public async Task<WorldGraphService> CreateWorldGraphServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        new(await rawRawWorldGraphServiceFactory.TryCreateServiceAsync(version, cancellationToken));
}
