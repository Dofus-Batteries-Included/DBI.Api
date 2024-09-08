using Microsoft.Extensions.Options;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Workers;
using Server.Infrastructure.Repository;

namespace Server.Domains.DataCenter;

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

        services.AddHostedService<DownloadDataFromGithubReleases>();

        services.AddOpenApiDocument(
            settings =>
            {
                settings.DocumentName = "ddc";
                settings.Title = "Dofus Data Center - API";
                settings.Description = "Data extracted from Dofus.";
                settings.Version = Metadata.Version?.ToString() ?? "~dev";
            }
        );
    }
}
