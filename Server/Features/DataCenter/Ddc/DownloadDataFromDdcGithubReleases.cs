using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using DBI.Server.Common.Workers;

namespace DBI.Server.Features.DataCenter.Ddc;

partial class DownloadDataFromDdcGithubReleases : PeriodicService
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly HashSet<string> _processedReleases = [];
    readonly RawDataFromDdcGithubReleasesSavedToDisk _repository;

    public DownloadDataFromDdcGithubReleases(IHttpClientFactory httpClientFactory, RawDataFromDdcGithubReleasesSavedToDisk repository, ILoggerFactory loggerFactory) : base(
        TimeSpan.FromHours(1),
        loggerFactory.CreateLogger<DownloadDataFromDdcGithubReleases>()
    )
    {
        _httpClientFactory = httpClientFactory;
        _repository = repository;
    }

    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Start refreshing data from DDC Releases.");

        await foreach (DdcRelease release in GetReleasesAsync(stoppingToken))
        {
            if (_processedReleases.Contains(release.Name))
            {
                Logger.LogDebug("Skipping release {Name} because it has already been processed.", release.Name);
                continue;
            }

            Stream? dataFile = await DownloadReleaseDataAsync(release, stoppingToken);
            if (dataFile == null)
            {
                Logger.LogWarning("Could not get data from release {Name}.", release.Name);
                continue;
            }

            await using Stream _ = dataFile;
            using ZipArchive zip = new(dataFile, ZipArchiveMode.Read);

            DdcMetadata? metadata = await ReadMetadataAsync(zip, stoppingToken);
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
                await _repository.SaveRawDataFilesAsync(release, metadata.GameVersion, zip, stoppingToken);
            }

            _processedReleases.Add(release.Name);
        }
    }

    async IAsyncEnumerable<DdcRelease> GetReleasesAsync([EnumeratorCancellation] CancellationToken stoppingToken)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        httpClient.DefaultRequestHeaders.Add("X-Github-Api-Version", "2022-11-28");

        string uri = "https://api.github.com/repos/Dofus-Batteries-Included/DDC/releases";
        while (true)
        {
            Logger.LogInformation("Will download releases from: {Uri}", uri);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(uri, stoppingToken);
            httpResponse.EnsureSuccessStatusCode();

            DdcRelease[]? responses = await httpResponse.Content.ReadFromJsonAsync<DdcRelease[]>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower },
                stoppingToken
            );
            if (responses == null)
            {
                Logger.LogError("Could not parse response from Github Releases.");
                break;
            }

            foreach (DdcRelease response in responses)
            {
                yield return response;
            }

            if (!httpResponse.Headers.TryGetValues("Link", out IEnumerable<string>? links))
            {
                break;
            }

            Regex nextLinkRegex = NextLinkRegex();
            Match? match = links.Select(l => nextLinkRegex.Match(l)).FirstOrDefault(m => m.Success);
            if (match == null)
            {
                break;
            }

            uri = match.Groups["uri"].Value;
        }
    }

    async Task<Stream?> DownloadReleaseDataAsync(DdcRelease release, CancellationToken stoppingToken)
    {
        DdcAsset? dataAsset = release.Assets.FirstOrDefault(a => a.Name == "data.zip");
        if (dataAsset == null)
        {
            return null;
        }

        using HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        HttpResponseMessage response = await httpClient.GetAsync(dataAsset.BrowserDownloadUrl, stoppingToken);
        return await response.Content.ReadAsStreamAsync(stoppingToken);
    }

    static async Task<DdcMetadata?> ReadMetadataAsync(ZipArchive zip, CancellationToken stoppingToken)
    {
        ZipArchiveEntry? metadataEntry = zip.GetEntry("metadata.json");
        if (metadataEntry == null)
        {
            return null;
        }

        await using Stream metadataStream = metadataEntry.Open();
        return await JsonSerializer.DeserializeAsync<DdcMetadata>(metadataStream, cancellationToken: stoppingToken);
    }

    [GeneratedRegex("<(?<uri>[^>]*)>; rel=\"next\"")]
    private static partial Regex NextLinkRegex();
}
