using System.IO.Compression;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.Ddc;

public class DdcClient
{
    static readonly Regex NextLinkRegex = new("<(?<uri>[^>]*)>; rel=\"next\"");
    readonly IHttpClientFactory? _httpClientFactory;
    readonly ILogger _logger;

    public DdcClient(IHttpClientFactory httpClientFactory, ILogger? logger = null) : this(logger)
    {
        _httpClientFactory = httpClientFactory;
    }

    public DdcClient(ILogger? logger = null)
    {
        _logger = logger ?? NullLogger<DdcClient>.Instance;
    }

    /// <summary>
    ///     The full name of the github repository to read data from.
    ///     Defaults to <c>Dofus-Batteries-Included/DDC</c>
    /// </summary>
    public string GithubRepository { get; set; } = "Dofus-Batteries-Included/DDC";

    public async IAsyncEnumerable<DdcRelease> GetReleasesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using HttpClient httpClient = _httpClientFactory?.CreateClient() ?? new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        httpClient.DefaultRequestHeaders.Add("X-Github-Api-Version", "2022-11-28");

        _logger.LogInformation("Start getting releases from github repository {Repository}", GithubRepository);

        Uri nextReleaseUri = new($"https://api.github.com/repos/{GithubRepository}/releases");
        while (true)
        {
            _logger.LogInformation("Will download releases from: {Uri}", nextReleaseUri);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(nextReleaseUri, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            DdcRelease[]? responses = await httpResponse.Content.ReadFromJsonAsync<DdcRelease[]>(
#if NET7_0_OR_GREATER
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower },
#else
                new JsonSerializerOptions { PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance },
#endif
                cancellationToken
            );
            if (responses == null)
            {
                _logger.LogError("Could not parse response from Github Releases.");
                break;
            }

            _logger.LogInformation("Found releases: {Releases}.", string.Join(", ", responses.Select(r => r.Name)));

            foreach (DdcRelease response in responses)
            {
                yield return response;
            }

            if (!httpResponse.Headers.TryGetValues("Link", out IEnumerable<string>? links))
            {
                break;
            }

            Match? match = links.Select(l => NextLinkRegex.Match(l)).FirstOrDefault(m => m.Success);
            if (match == null)
            {
                break;
            }

            nextReleaseUri = new Uri(match.Groups["uri"].Value);
            _logger.LogInformation("Found link to more releases at {Uri}.", nextReleaseUri);
        }

        _logger.LogInformation("Done getting releases from github repository {Repository}.", GithubRepository);
    }

    public async Task<DdcReleaseContent?> DownloadReleaseContentAsync(DdcRelease release, CancellationToken cancellationToken = default)
    {
        DdcAsset? dataAsset = release.Assets.FirstOrDefault(a => a.Name == "data.zip");
        if (dataAsset == null)
        {
            return null;
        }

        using HttpClient httpClient = _httpClientFactory?.CreateClient() ?? new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        using HttpResponseMessage response = await httpClient.GetAsync(dataAsset.BrowserDownloadUrl, cancellationToken);

        // copy release content to memory
        await using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        MemoryStream memoryStream = new();
        await contentStream.CopyToAsync(memoryStream, cancellationToken);

        ZipArchive zip = new(memoryStream, ZipArchiveMode.Read, false);
        return new DdcReleaseContent(zip);
    }

#if !NET7_0_OR_GREATER
    /// <remarks>
    ///     The System.Text.Json is not built-in before net7.0.
    ///     For net6.0, the nuget package is required to use it. However, the nuget package does not define the snake case naming policy.
    /// </remarks>
    class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new();
        public override string ConvertName(string name) => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }
#endif
}
