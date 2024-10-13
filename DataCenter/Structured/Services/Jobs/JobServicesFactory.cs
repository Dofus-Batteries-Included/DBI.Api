using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Jobs;
using DBI.DataCenter.Raw.Services.Recipes;
using DBI.DataCenter.Raw.Services.Skills;
using DBI.DataCenter.Structured.Services.Items;

namespace DBI.DataCenter.Structured.Services.Jobs;

public class JobServicesFactory(
    RawJobsServiceFactory rawJobsService,
    RawRecipesServiceFactory rawRecipesServiceFactory,
    RawSkillNamesServiceFactory rawSkillNamesServiceFactory,
    ItemServicesFactory itemServicesFactory,
    LanguagesServiceFactory languagesServiceFactory
)
{
    /// <summary>
    ///     Create an instance of JobsService for the given version of the game.
    /// </summary>
    public async Task<JobsService> CreateJobsServiceAsync(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(await rawJobsService.TryCreateServiceAsync(gameVersion, cancellationToken), await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken));

    /// <summary>
    ///     Create an instance of RecipesService for the given version of the game.
    /// </summary>
    public async Task<RecipesService> CreateRecipesServiceAsync(string gameVersion = "latest", CancellationToken cancellationToken = default) =>
        new(
            await rawRecipesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await rawSkillNamesServiceFactory.TryCreateServiceAsync(gameVersion, cancellationToken),
            await CreateJobsServiceAsync(gameVersion, cancellationToken),
            await itemServicesFactory.CreateItemsServiceAsync(gameVersion, cancellationToken),
            await languagesServiceFactory.CreateLanguagesServiceAsync(gameVersion, cancellationToken)
        );
}
