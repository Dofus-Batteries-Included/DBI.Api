using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Items;
using DBI.DataCenter.Structured.Services.World;

namespace DBI.DataCenter.Structured.Services.Items;

public class ItemServicesFactory(
    RawEvolutiveItemTypesServiceFactory rawEvolutiveItemTypesServiceFactory,
    RawItemTypesServiceFactory rawItemTypesServiceFactory,
    RawItemsServiceFactory rawItemsServiceFactory,
    RawItemSetsServiceFactory rawItemSetsServiceFactory,
    LanguagesServiceFactory languagesServiceFactory,
    WorldServicesFactory worldServicesFactory
)
{
    public async Task<ItemTypesService> CreateItemTypesServiceAsync(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawEvolutiveItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken)
        );

    public async Task<ItemsService> CreateItemsServiceAsync(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawItemsServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawItemSetsServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken)
        );

    public async Task<ItemsRecyclingDataService> CreateItemRecyclingDataServiceAsync(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawItemsServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await worldServicesFactory.CreateSubAreasServiceAsync(gameVersion, cancellationToken),
            await CreateItemsServiceAsync(gameVersion, cancellationToken)
        );
}
