using MediatR;

namespace DBI.Server.Common.Notifications;

class LatestVersionChangedNotification : INotification
{
    /// <summary>
    ///     The latest version before the change, if any.
    ///     Null means that the change is the first one, e.g. the first time the application is executed.
    /// </summary>
    public required string? OldLatestVersion { get; init; }

    /// <summary>
    ///     The latest version after the change.
    /// </summary>
    public required string NewLatestVersion { get; init; }
}
