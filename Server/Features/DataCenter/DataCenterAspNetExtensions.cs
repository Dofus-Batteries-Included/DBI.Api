using DBI.Server.Common.OpenApi;
using DBI.Server.Features.DataCenter.Raw.Services.I18N;
using DBI.Server.Features.DataCenter.Raw.Services.Maps;
using DBI.Server.Features.DataCenter.Raw.Services.PointOfInterests;
using DBI.Server.Features.DataCenter.Raw.Services.WorldGraphs;
using DBI.Server.Features.DataCenter.Repositories;
using DBI.Server.Features.DataCenter.Services;
using DBI.Server.Features.DataCenter.Workers;
using DBI.Server.Infrastructure.Repository;
using Microsoft.Extensions.Options;

namespace DBI.Server.Features.DataCenter;

static class DataCenterAspNetExtensions
{
    public static void ConfigureDataCenter(this IServiceCollection services)
    {
        services.AddSingleton<RawDataFromGithubReleasesSavedToDisk>(
            s => new RawDataFromGithubReleasesSavedToDisk(
                s.GetRequiredService<IOptions<RepositoryOptions>>(),
                s.GetRequiredService<ILogger<RawDataFromGithubReleasesSavedToDisk>>()
            )
        );
        services.AddSingleton<IRawDataRepository, RawDataFromGithubReleasesSavedToDisk>(s => s.GetRequiredService<RawDataFromGithubReleasesSavedToDisk>());
        services.AddSingleton<LanguagesServiceFactory>();
        services.AddSingleton<RawWorldMapsServiceFactory>();
        services.AddSingleton<RawSuperAreasServiceFactory>();
        services.AddSingleton<RawAreasServiceFactory>();
        services.AddSingleton<RawSubAreasServiceFactory>();
        services.AddSingleton<RawMapsServiceFactory>();
        services.AddSingleton<RawMapPositionsServiceFactory>();
        services.AddSingleton<RawPointOfInterestsServiceFactory>();
        services.AddSingleton<RawWorldGraphServiceFactory>();

        services.AddSingleton<WorldServiceFactory>();

        services.AddHostedService<DownloadDataFromGithubReleases>();

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
