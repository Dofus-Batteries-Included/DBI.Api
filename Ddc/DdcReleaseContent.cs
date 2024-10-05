using System.IO.Compression;
using System.Text.Json;

namespace DBI.Ddc;

public class DdcReleaseContent : IDisposable
{
    const string MetadataFileName = "metadata.json";
    readonly ZipArchive _archive;

    internal DdcReleaseContent(ZipArchive archive)
    {
        _archive = archive;
    }

    public async Task<DdcMetadata?> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        ZipArchiveEntry? metadataEntry = _archive.GetEntry(MetadataFileName);
        if (metadataEntry == null)
        {
            return null;
        }

        await using Stream metadataStream = metadataEntry.Open();
        return await JsonSerializer.DeserializeAsync<DdcMetadata>(metadataStream, cancellationToken: cancellationToken);
    }

    public Task<IReadOnlyList<string>> GetFilesAsync() =>
        Task.FromResult<IReadOnlyList<string>>(_archive.Entries.Where(e => e.Name != MetadataFileName).Select(e => e.FullName).ToArray());

    public Task<Stream?> GetFileContentAsync(string filename)
    {
        ZipArchiveEntry? entry = _archive.GetEntry(filename);
        return entry == null ? Task.FromResult<Stream?>(null) : Task.FromResult<Stream?>(entry.Open());
    }

    public void Dispose()
    {
        _archive.Dispose();
        GC.SuppressFinalize(this);
    }
}
