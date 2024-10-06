using MediatR;

namespace DBI.Server.Common.Notifications;

/// <summary>
///     Triggered when a new version is downloaded and available. The new version is not necessarily the latest one, it can be an old one that has just been discovered.
/// </summary>
class NewVersionAvailableNotification : INotification
{
    /// <summary>
    ///     The latest version before the change, if any.
    ///     This might or might not be <see cref="NewLatestVersion" />.
    ///     This might also be null, meaning that the version that is available is the first version that has ever been discovered by the application, e.g. the first time the application
    ///     is executed.
    /// </summary>
    public required string? OldLatestVersion { get; init; }

    /// <summary>
    ///     The latest version after the change.
    ///     This might or might not be <see cref="OldLatestVersion" />
    /// </summary>
    public required string NewLatestVersion { get; init; }

    /// <summary>
    ///     The new version that is available.
    ///     This might or might not be the <see cref="NewLatestVersion" />.
    /// </summary>
    public required string NewVersion { get; init; }
}
