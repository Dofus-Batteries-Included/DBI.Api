using MediatR;

namespace DBI.Server.Common.Notifications;

class LatestVersionChangedNotification : INotification
{
    /// <summary>
    ///     The new latest version after it has changed.
    /// </summary>
    public required string NewLatestVersion { get; init; }
}
