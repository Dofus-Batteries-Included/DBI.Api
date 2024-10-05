using System.Diagnostics;
using System.Reflection;
using Semver;

namespace DBI.Server;

static class Metadata
{
    public static SemVersion? Version { get; private set; }

    static Metadata()
    {
        Assembly assembly = typeof(Metadata).Assembly;
        string? versionStr = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        Version = versionStr != null ? SemVersion.Parse(versionStr, SemVersionStyles.Any) : null;
    }
}

static class MetadataVersionExtensions
{
    public static bool IsDebug(this SemVersion version) => version.MetadataIdentifiers.Any(m => string.Equals(m.Value, "debug", StringComparison.InvariantCultureIgnoreCase));

    public static string PrereleaseIdentifier(this SemVersion version) =>
        string.IsNullOrWhiteSpace(version.Prerelease) ? "" : version.Prerelease[..version.Prerelease.IndexOf('.')];
}
