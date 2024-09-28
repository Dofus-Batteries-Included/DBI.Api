using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Server.Common.Exceptions;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Workers;
using Server.Infrastructure.Repository;

namespace Server.Features.DataCenter.Repositories;

partial class RawDataFromGithubReleasesSavedToDisk : IRawDataRepository
{
    readonly IOptions<RepositoryOptions> _repositoryOptions;
    readonly ILogger<RawDataFromGithubReleasesSavedToDisk> _logger;
    readonly JsonSerializerOptions _ddcMetadataJsonSerializerOptions = new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };

    public RawDataFromGithubReleasesSavedToDisk(IOptions<RepositoryOptions> repositoryOptions, ILogger<RawDataFromGithubReleasesSavedToDisk> logger)
    {
        _repositoryOptions = repositoryOptions;
        _logger = logger;
    }

    public event EventHandler? LatestVersionChanged;

    public Task<string> GetLatestVersionAsync() =>
        Task.FromResult(GetActualVersions().OrderDescending().FirstOrDefault() ?? throw new NotFoundException("Could not find any version."));

    public Task<IReadOnlyCollection<string>> GetAvailableVersionsAsync() => Task.FromResult<IReadOnlyCollection<string>>(GetActualVersions().ToList());

    public Task<IRawDataFile> GetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default)
    {
        (IRawDataFile? file, string? errorMessage) = TryGetRawDataFileImpl(version, type);
        if (file == null)
        {
            throw new NotFoundException(errorMessage ?? "Could not find data.");
        }

        return Task.FromResult(file);
    }

    public Task<IRawDataFile?> TryGetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default)
    {
        (IRawDataFile? file, string? _) = TryGetRawDataFileImpl(version, type);
        return Task.FromResult(file);
    }

    (IRawDataFile? file, string? ErrorMessage) TryGetRawDataFileImpl(string version, RawDataType type)
    {
        string? actualVersion = GetActualVersion(version);
        if (actualVersion == null)
        {
            return (null, $"Could not find data for version {version}.");
        }

        string versionPath = Path.Join(_repositoryOptions.Value.DataCenterRawDataPath, actualVersion);
        if (!Directory.Exists(versionPath))
        {
            return (null, $"Could not find data for version {actualVersion}.");
        }

        string path = Path.Join(versionPath, GetFilename(type));
        if (!Path.Exists(path))
        {
            return (null, $"Could not find data for {type} in version {actualVersion}.");
        }

        return (new File(path, actualVersion), null);
    }

    public async Task<SavedDataSummary> GetSavedDataSummaryAsync(CancellationToken cancellationToken = default)
    {
        List<string> versions = GetActualVersions().ToList();
        Dictionary<string, DdcMetadata> versionsMetadata = new();
        foreach (string version in versions)
        {
            string path = Path.Join(_repositoryOptions.Value.DataCenterRawDataPath, version);
            DdcMetadata? metadata = await ReadDdcMetadataAsync(path, cancellationToken);
            if (metadata == null)
            {
                _logger.LogWarning("Could not find DDC metadata at {Path}.", path);
                continue;
            }

            versionsMetadata[version] = metadata;
        }

        return new SavedDataSummary(versions, versionsMetadata);
    }

    public async Task<DdcMetadata?> GetSavedMetadataAsync(string version, CancellationToken cancellationToken = default)
    {
        string path = Path.Join(_repositoryOptions.Value.DataCenterRawDataPath, version);
        return await ReadDdcMetadataAsync(path, cancellationToken);
    }

    public async Task SaveRawDataFilesAsync(DownloadDataFromGithubReleases.Release release, string gameVersion, ZipArchive archive, CancellationToken cancellationToken = default)
    {
        string? oldLatest = GetActualVersion("latest");

        _logger.LogInformation("Saving data from release {Name} containing version {Version}.", release.Name, gameVersion);

        string path = Path.Join(_repositoryOptions.Value.DataCenterRawDataPath, gameVersion);
        Directory.CreateDirectory(path);

        await WriteDdcMetadataAsync(release, path, cancellationToken);

        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            string entryFullPath = Path.Join(path, entry.FullName);
            string? entryDirectory = Path.GetDirectoryName(entryFullPath);
            if (entryDirectory != null && !Directory.Exists(entryDirectory))
            {
                Directory.CreateDirectory(entryFullPath);
            }

            _logger.LogDebug("Writing file {Path}...", entryFullPath);

            await using FileStream file = System.IO.File.OpenWrite(entryFullPath);
            await using Stream entryStream = entry.Open();
            await entryStream.CopyToAsync(file, cancellationToken);
        }

        if (oldLatest != null && string.CompareOrdinal(gameVersion, oldLatest) > 0)
        {
            LatestVersionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    string? GetActualVersion(string version) =>
        version switch
        {
            "latest" => GetActualVersions().OrderDescending().FirstOrDefault(),
            _ => version
        };

    static string GetFilename(RawDataType type) =>
        type switch
        {
            RawDataType.I18NFr => "fr.i18n.json",
            RawDataType.I18NEn => "en.i18n.json",
            RawDataType.I18NEs => "es.i18n.json",
            RawDataType.I18NDe => "de.i18n.json",
            RawDataType.I18NPt => "pt.i18n.json",
            RawDataType.MapPositions => "map-positions.json",
            RawDataType.MapCoordinates => "map-coordinates.json",
            RawDataType.PointOfInterest => "point-of-interest.json",
            RawDataType.WorldGraph => "world-graph.json",
            RawDataType.SuperAreas => "super-areas.json",
            RawDataType.Areas => "areas.json",
            RawDataType.SubAreas => "sub-areas.json",
            RawDataType.Maps => "maps.json",
            RawDataType.WorldMaps => "world-maps.json",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    IEnumerable<string> GetActualVersions()
    {
        Regex versionRegex = VersionRegex();
        return Directory.Exists(_repositoryOptions.Value.DataCenterRawDataPath)
            ? Directory.EnumerateDirectories(_repositoryOptions.Value.DataCenterRawDataPath).Select(Path.GetFileName).OfType<string>().Where(v => versionRegex.IsMatch(v)).Order()
            : [];
    }

    async Task WriteDdcMetadataAsync(DownloadDataFromGithubReleases.Release release, string directory, CancellationToken cancellationToken)
    {
        DdcMetadata ddcMetadata = new() { ReleaseUrl = release.HtmlUrl, ReleaseName = release.Name };
        string ddcMetadataPath = Path.Join(directory, "ddc-metadata.json");
        await using FileStream ddcMetadataStream = System.IO.File.OpenWrite(ddcMetadataPath);
        await JsonSerializer.SerializeAsync(ddcMetadataStream, ddcMetadata, _ddcMetadataJsonSerializerOptions, cancellationToken);
    }

    async Task<DdcMetadata?> ReadDdcMetadataAsync(string directory, CancellationToken cancellationToken)
    {
        string ddcMetadataPath = Path.Join(directory, "ddc-metadata.json");
        if (!System.IO.File.Exists(ddcMetadataPath))
        {
            return null;
        }

        await using FileStream ddcMetadataStream = System.IO.File.OpenRead(ddcMetadataPath);
        return await JsonSerializer.DeserializeAsync<DdcMetadata>(ddcMetadataStream, _ddcMetadataJsonSerializerOptions, cancellationToken);
    }

    class File : IRawDataFile
    {
        readonly string _filepath;

        public File(string filepath, string version)
        {
            _filepath = filepath;
            Version = version;
            Name = Path.GetFileName(filepath);
        }

        public string Name { get; }
        public string Version { get; }
        public string ContentType { get; } = "application/json";
        public Stream OpenRead() => System.IO.File.OpenRead(_filepath);
    }

    public class SavedDataSummary
    {
        readonly IReadOnlyList<string> _versions;
        readonly Dictionary<string, DdcMetadata> _metadata;

        public SavedDataSummary(IReadOnlyList<string> versions, Dictionary<string, DdcMetadata> metadata)
        {
            _versions = versions;
            _metadata = metadata;
        }

        public IReadOnlyList<string> GetVersions() => _versions;
        public DdcMetadata? GetMetadata(string version) => _metadata.GetValueOrDefault(version);
    }

    public class DdcMetadata
    {
        public required string ReleaseUrl { get; init; }
        public required string ReleaseName { get; init; }
    }

    [GeneratedRegex(
        @"^\d+(\.\d+)*$",
        RegexOptions.Compiled | RegexOptions.NonBacktracking | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.ExplicitCapture
    )]
    private static partial Regex VersionRegex();
}
