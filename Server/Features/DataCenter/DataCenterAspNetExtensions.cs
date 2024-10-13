using DBI.DataCenter.Raw;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Items;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.PointOfInterests;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.Server.Common.OpenApi;
using DBI.Server.Infrastructure;
using MediatR;
using Microsoft.Extensions.Options;

namespace DBI.Server.Features.DataCenter;

static class DataCenterAspNetExtensions
{
    public static void ConfigureDataCenter(this IServiceCollection services)
    {
        services.AddSingleton<RawDataFromDdcGithubReleasesSavedToDisk>(
            s => new RawDataFromDdcGithubReleasesSavedToDisk(
                s.GetRequiredService<IOptions<RepositoryOptions>>().Value,
                s.GetRequiredService<IMediator>(),
                s.GetRequiredService<ILogger<RawDataFromDdcGithubReleasesSavedToDisk>>()
            )
        );

        services.AddSingleton<IRawDataRepository>(s => s.GetRequiredService<RawDataFromDdcGithubReleasesSavedToDisk>());

        services.AddSingleton<RawDataJsonOptionsProvider>();
        services.AddSingleton<LanguagesServiceFactory>();
        services.AddSingleton<RawWorldMapsServiceFactory>();
        services.AddSingleton<RawSuperAreasServiceFactory>();
        services.AddSingleton<RawAreasServiceFactory>();
        services.AddSingleton<RawSubAreasServiceFactory>();
        services.AddSingleton<RawMapsServiceFactory>();
        services.AddSingleton<RawMapPositionsServiceFactory>();
        services.AddSingleton<RawPointOfInterestsServiceFactory>();
        services.AddSingleton<RawWorldGraphServiceFactory>();
        services.AddSingleton<RawItemsServiceFactory>();
        services.AddSingleton<RawItemSetsServiceFactory>();
        services.AddSingleton<RawItemTypesServiceFactory>();
        services.AddSingleton<RawItemSuperTypesServiceFactory>();
        services.AddSingleton<RawEvolutiveItemTypesServiceFactory>();

        services.AddSingleton<WorldServiceFactory>();

        services.AddHostedService<DownloadDataFromDdcGithubReleases>();

        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "data-center";
                settings.Title = "Dofus Data Center - API";
                settings.Description = "Data extracted from Dofus.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
                settings.OperationProcessors.Insert(0, new FilterOperationsByRoutePrefix("/data-center"));
                settings.DocumentProcessors.Add(new SortTagsInDocument());
            }
        );
    }
}
