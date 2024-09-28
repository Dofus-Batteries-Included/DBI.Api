using Microsoft.Extensions.Options;
using Server.Common.OpenApi;
using Server.Features.DataCenter.Raw.Services.I18N;
using Server.Features.DataCenter.Raw.Services.Maps;
using Server.Features.DataCenter.Raw.Services.PointOfInterests;
using Server.Features.DataCenter.Raw.Services.WorldGraphs;
using Server.Features.DataCenter.Repositories;
using Server.Features.DataCenter.Services;
using Server.Features.DataCenter.Workers;
using Server.Infrastructure.Repository;

namespace Server.Features.DataCenter;

public static class DataCenterAspNetExtensions
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
        services.AddSingleton<RawSuperAreasServiceFactory>();
        services.AddSingleton<RawAreasServiceFactory>();
        services.AddSingleton<RawSubAreasServiceFactory>();
        services.AddSingleton<RawMapsServiceFactory>();
        services.AddSingleton<RawMapPositionsServiceFactory>();
        services.AddSingleton<RawPointOfInterestsServiceFactory>();
        services.AddSingleton<WorldGraphServiceFactory>();

        services.AddSingleton<MapsServiceFactory>();

        services.AddHostedService<DownloadDataFromGithubReleases>();

        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "data-center";
                settings.Title = "Dofus Data Center - API";
                settings.Description = "Data extracted from Dofus.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
                settings.OperationProcessors.Insert(0, new FilterOperationsByRoutePrefix("/data-center"));
            }
        );
    }
}
