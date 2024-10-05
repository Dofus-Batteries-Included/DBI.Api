namespace DBI.Server.Features.DataCenter.Ddc;

/// <summary>
///     Metadata in a DDC release.
/// </summary>
/// <seealso cref="DdcRelease" />
class DdcMetadata
{
    public required string GameVersion { get; init; }
}
