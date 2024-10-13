using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Items;

namespace DBI.DataCenter.Structured.Services.Items;

public class ItemServicesFactory(
    RawEvolutiveItemTypesServiceFactory rawEvolutiveItemTypesServiceFactory,
    RawItemTypesServiceFactory rawItemTypesServiceFactory,
    RawItemsServiceFactory rawItemsServiceFactory,
    RawItemSetsServiceFactory rawItemSetsServiceFactory,
    LanguagesServiceFactory languagesServiceFactory
)
{
    public async Task<ItemTypesService> CreateItemTypesService(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawEvolutiveItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken)
        );

    public async Task<ItemsService> CreateItemsService(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawItemTypesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawItemsServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawItemSetsServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken)
        );
}
