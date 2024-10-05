﻿using System.IO.Compression;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace DBI.Ddc;

public partial class DdcClient
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly ILogger<DdcClient> _logger;

    public DdcClient(IHttpClientFactory httpClientFactory, ILogger<DdcClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    ///     The full name of the github repository to read data from.
    ///     Defaults to <c>Dofus-Batteries-Included/DDC</c>
    /// </summary>
    public string GithubRepository { get; set; } = "Dofus-Batteries-Included/DDC";

    public async IAsyncEnumerable<DdcRelease> GetReleasesAsync([EnumeratorCancellation] CancellationToken stoppingToken)
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        httpClient.DefaultRequestHeaders.Add("X-Github-Api-Version", "2022-11-28");

        _logger.LogInformation("Start getting releases from github repository {Repository}", GithubRepository);

        Uri nextReleaseUri = new($"https://api.github.com/repos/{GithubRepository}/releases");
        while (true)
        {
            _logger.LogInformation("Will download releases from: {Uri}", nextReleaseUri);

            HttpResponseMessage httpResponse = await httpClient.GetAsync(nextReleaseUri, stoppingToken);
            httpResponse.EnsureSuccessStatusCode();

            DdcRelease[]? responses = await httpResponse.Content.ReadFromJsonAsync<DdcRelease[]>(
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower },
                stoppingToken
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

            Regex nextLinkRegex = NextLinkRegex();
            Match? match = links.Select(l => nextLinkRegex.Match(l)).FirstOrDefault(m => m.Success);
            if (match == null)
            {
                break;
            }

            nextReleaseUri = new Uri(match.Groups["uri"].Value);
            _logger.LogInformation("Found link to more releases at {Uri}.", nextReleaseUri);
        }

        _logger.LogInformation("Done getting releases from github repository {Repository}.", GithubRepository);
    }

    public async Task<DdcReleaseContent?> DownloadReleaseContentAsync(DdcRelease release, CancellationToken stoppingToken)
    {
        DdcAsset? dataAsset = release.Assets.FirstOrDefault(a => a.Name == "data.zip");
        if (dataAsset == null)
        {
            return null;
        }

        using HttpClient httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "DDC-Api");
        using HttpResponseMessage response = await httpClient.GetAsync(dataAsset.BrowserDownloadUrl, stoppingToken);

        // copy release content to memory
        await using Stream contentStream = await response.Content.ReadAsStreamAsync(stoppingToken);
        MemoryStream memoryStream = new();
        await contentStream.CopyToAsync(memoryStream, stoppingToken);

        ZipArchive zip = new(memoryStream, ZipArchiveMode.Read, false);
        return new DdcReleaseContent(zip);
    }

    [GeneratedRegex("<(?<uri>[^>]*)>; rel=\"next\"")]
    private static partial Regex NextLinkRegex();
}
