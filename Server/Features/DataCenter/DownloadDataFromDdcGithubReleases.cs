using DBI.Ddc;
using DBI.Server.Common.Workers;

namespace DBI.Server.Features.DataCenter;

class DownloadDataFromDdcGithubReleases : PeriodicService
{
    readonly HashSet<string> _processedReleases = [];
    readonly IHttpClientFactory _httpClientFactory;
    readonly RawDataFromDdcGithubReleasesSavedToDisk _repository;
    readonly ILoggerFactory _loggerFactory;

    public DownloadDataFromDdcGithubReleases(IHttpClientFactory httpClientFactory, RawDataFromDdcGithubReleasesSavedToDisk repository, ILoggerFactory loggerFactory) : base(
        TimeSpan.FromHours(1),
        loggerFactory.CreateLogger<DownloadDataFromDdcGithubReleases>()
    )
    {
        _httpClientFactory = httpClientFactory;
        _repository = repository;
        _loggerFactory = loggerFactory;
    }

    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Start refreshing data from DDC Releases.");

        DdcClient ddcClient = new(_httpClientFactory, _loggerFactory.CreateLogger<DdcClient>());

        await foreach (DdcRelease release in ddcClient.GetReleasesAsync(stoppingToken))
        {
            if (_processedReleases.Contains(release.Name))
            {
                Logger.LogDebug("Skipping release {Name} because it has already been processed.", release.Name);
                continue;
            }

            using DdcReleaseContent? releaseContent = await ddcClient.DownloadReleaseContentAsync(release, stoppingToken);
            if (releaseContent == null)
            {
                Logger.LogWarning("Could not get data from release {Name}.", release.Name);
                continue;
            }

            DdcMetadata? metadata = await releaseContent.GetMetadataAsync(stoppingToken);
            if (metadata == null)
            {
                Logger.LogWarning($"Could not get metadata in data from release {release.Name}.");
                continue;
            }

            RawDataFromDdcGithubReleasesSavedToDisk.Metadata? metadataSavedToDisk = await _repository.GetSavedMetadataAsync(metadata.GameVersion, stoppingToken);
            bool ignoreRelease = metadataSavedToDisk != null && string.Compare(release.Name, metadataSavedToDisk.ReleaseName, StringComparison.InvariantCultureIgnoreCase) <= 0;

            if (ignoreRelease)
            {
                Logger.LogInformation(
                    "Release {ReleaseName} containing data for version {Version} will be ignored because the current data has been extracted from more recent release {MetadataReleaseName}.",
                    release.Name,
                    metadata.GameVersion,
                    metadataSavedToDisk!.ReleaseName
                );
            }
            else
            {
                await _repository.SaveRawDataFilesAsync(release, releaseContent, stoppingToken);
            }

            _processedReleases.Add(release.Name);
        }
    }
}
