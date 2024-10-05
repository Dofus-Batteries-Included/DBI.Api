using DBI.DataCenter.Raw;
using DBI.DataCenter.Raw.Ddc;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.PointOfInterests;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.Server.Common.OpenApi;
using DBI.Server.Features.DataCenter.Workers;
using Microsoft.Extensions.Options;

namespace DBI.Server.Features.DataCenter;

static class DataCenterAspNetExtensions
{
    public static void ConfigureDataCenter(this IServiceCollection services)
    {
        services.AddSingleton<RawDataFromDdcGithubReleasesSavedToDisk>(
            s => new RawDataFromDdcGithubReleasesSavedToDisk(
                s.GetRequiredService<IOptions<RepositoryOptions>>().Value,
                s.GetRequiredService<ILogger<RawDataFromDdcGithubReleasesSavedToDisk>>()
            )
        );
        services.AddSingleton<IRawDataRepository, RawDataFromDdcGithubReleasesSavedToDisk>(s => s.GetRequiredService<RawDataFromDdcGithubReleasesSavedToDisk>());
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
