﻿using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using DBI.DataCenter.Raw;
using DBI.DataCenter.Raw.Models;
using DBI.Ddc;
using DBI.Server.Common.Exceptions;
using DBI.Server.Common.Notifications;
using DBI.Server.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.Server.Features.DataCenter;

partial class RawDataFromDdcGithubReleasesSavedToDisk : IRawDataRepository
{
    readonly RepositoryOptions _repositoryOptions;
    readonly IMediator _mediator;
    readonly ILogger<RawDataFromDdcGithubReleasesSavedToDisk> _logger;
    readonly JsonSerializerOptions _ddcMetadataJsonSerializerOptions = new() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower };

    public RawDataFromDdcGithubReleasesSavedToDisk(RepositoryOptions repositoryOptions, IMediator mediator, ILogger<RawDataFromDdcGithubReleasesSavedToDisk>? logger = null)
    {
        _repositoryOptions = repositoryOptions;
        _mediator = mediator;
        _logger = logger ?? NullLogger<RawDataFromDdcGithubReleasesSavedToDisk>.Instance;
    }

    public Task<string?> GetLatestVersionAsync() => Task.FromResult(GetActualVersions().OrderDescending().FirstOrDefault());

    public Task<IReadOnlyCollection<string>> GetAvailableVersionsAsync() => Task.FromResult<IReadOnlyCollection<string>>(GetActualVersions().ToList());

    public Task<IRawDataFile> GetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default)
    {
        (IRawDataFile? file, string? errorMessage) = TryGetRawDataFileImpl(version, type);
        if (file == null)
        {
            throw new NotFoundException(errorMessage);
        }

        return Task.FromResult(file);
    }

    public Task<IRawDataFile?> TryGetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default)
    {
        (IRawDataFile? file, string? _) = TryGetRawDataFileImpl(version, type);
        return Task.FromResult(file);
    }

    public async Task<Metadata?> GetSavedMetadataAsync(string version, CancellationToken cancellationToken = default)
    {
        string path = Path.Join(_repositoryOptions.DataCenterRawDataPath, version);
        return await ReadDdcMetadataAsync(path, cancellationToken);
    }

    public async Task SaveRawDataFilesAsync(DdcRelease release, DdcReleaseContent content, CancellationToken cancellationToken = default)
    {
        string? oldLatest = GetActualVersion("latest");
        string gameVersion = (await content.GetMetadataAsync(cancellationToken))?.GameVersion
                             ?? throw new InvalidOperationException("Could not find new game version in release content.");

        _logger.LogInformation("Saving data from release {Name} containing version {Version}.", release.Name, gameVersion);

        string path = Path.Join(_repositoryOptions.DataCenterRawDataPath, gameVersion);
        Directory.CreateDirectory(path);

        await WriteDdcMetadataAsync(release, path, cancellationToken);

        foreach (string filename in await content.GetFilesAsync())
        {
            Stream? contentStream = await content.GetFileContentAsync(filename);
            if (contentStream == null)
            {
                _logger.LogWarning("Could not get content of file {Filename} in release {Release}.", filename, release.Name);
                continue;
            }

            await using Stream _ = contentStream;

            string fileName = Path.GetFileNameWithoutExtension(filename);
            string entryFullPath = Path.Join(path, $"{fileName}.bin");
            string? entryDirectory = Path.GetDirectoryName(entryFullPath);
            if (entryDirectory != null && !Directory.Exists(entryDirectory))
            {
                Directory.CreateDirectory(entryFullPath);
            }

            _logger.LogDebug("Writing file {Path}...", entryFullPath);


            await using BrotliStream encodeStream = new(System.IO.File.OpenWrite(entryFullPath), CompressionLevel.Optimal, false);
            await contentStream.CopyToAsync(encodeStream, cancellationToken);
        }

        string newLatest = GetActualVersion("latest") ?? gameVersion;

        if (newLatest == gameVersion)
        {
            _logger.LogInformation(
                "A new version of the raw data files is available: {Version}, and is now the latest version. The old latest version was: {OldLatestVersion}.",
                gameVersion,
                oldLatest
            );
        }
        else
        {
            _logger.LogInformation("A new version of the raw data files is available: {Version}. The latest version is: {LatestVersion}.", gameVersion, newLatest);
        }

        await _mediator.Publish(new NewVersionAvailableNotification { OldLatestVersion = oldLatest, NewLatestVersion = newLatest, NewVersion = gameVersion }, cancellationToken);
    }

    (IRawDataFile? file, string? ErrorMessage) TryGetRawDataFileImpl(string version, RawDataType type)
    {
        string? actualVersion = GetActualVersion(version);
        if (actualVersion == null)
        {
            return (null, $"Could not find data for version {version}.");
        }

        string versionPath = Path.Join(_repositoryOptions.DataCenterRawDataPath, actualVersion);
        if (!Directory.Exists(versionPath))
        {
            return (null, $"Could not find data for version {actualVersion}.");
        }

        string filename = GetFilename(type);

        string compressedFileName = $"{Path.GetFileNameWithoutExtension(filename)}.bin";
        string compressedFilePath = Path.Join(versionPath, compressedFileName);
        if (Path.Exists(compressedFilePath))
        {
            return (new BrotliCompressedFile(compressedFilePath, actualVersion), null);
        }

        string path = Path.Join(versionPath, filename);
        if (Path.Exists(path))
        {
            return (new File(path, actualVersion), null);
        }

        return (null, $"Could not find data for {type} in version {version}.");
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
        return Directory.Exists(_repositoryOptions.DataCenterRawDataPath)
            ? Directory.EnumerateDirectories(_repositoryOptions.DataCenterRawDataPath).Select(Path.GetFileName).OfType<string>().Where(v => versionRegex.IsMatch(v)).Order()
            : [];
    }

    async Task WriteDdcMetadataAsync(DdcRelease release, string directory, CancellationToken cancellationToken)
    {
        Metadata metadata = new() { ReleaseUrl = release.HtmlUrl, ReleaseName = release.Name };
        string ddcMetadataPath = Path.Join(directory, "ddc-metadata.json");
        await using FileStream ddcMetadataStream = System.IO.File.OpenWrite(ddcMetadataPath);
        await JsonSerializer.SerializeAsync(ddcMetadataStream, metadata, _ddcMetadataJsonSerializerOptions, cancellationToken);
    }

    async Task<Metadata?> ReadDdcMetadataAsync(string directory, CancellationToken cancellationToken)
    {
        string ddcMetadataPath = Path.Join(directory, "ddc-metadata.json");
        if (!System.IO.File.Exists(ddcMetadataPath))
        {
            return null;
        }

        await using FileStream ddcMetadataStream = System.IO.File.OpenRead(ddcMetadataPath);
        return await JsonSerializer.DeserializeAsync<Metadata>(ddcMetadataStream, _ddcMetadataJsonSerializerOptions, cancellationToken);
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

    class BrotliCompressedFile : IRawDataFile
    {
        readonly string _filepath;

        public BrotliCompressedFile(string filepath, string version)
        {
            _filepath = filepath;
            Version = version;
            Name = $"{Path.GetFileNameWithoutExtension(filepath)}.json";
        }

        public string Name { get; }
        public string Version { get; }
        public string ContentType { get; } = "application/json";
        public Stream OpenRead() => new BrotliStream(System.IO.File.OpenRead(_filepath), CompressionMode.Decompress, false);
    }

    public class SavedDataSummary
    {
        readonly IReadOnlyList<string> _versions;
        readonly Dictionary<string, Metadata> _metadata;

        public SavedDataSummary(IReadOnlyList<string> versions, Dictionary<string, Metadata> metadata)
        {
            _versions = versions;
            _metadata = metadata;
        }

        public IReadOnlyList<string> GetVersions() => _versions;
        public Metadata? GetMetadata(string version) => _metadata.GetValueOrDefault(version);
    }

    public class Metadata
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
