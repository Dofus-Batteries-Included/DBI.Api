namespace DBI.Server.Features.DataCenter.Ddc;

/// <summary>
///     Asset in a DDC release.
/// </summary>
/// <seealso cref="DdcRelease" />
class DdcAsset
{
    public required string Name { get; init; }
    public required string BrowserDownloadUrl { get; init; }
}
