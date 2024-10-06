using DBI.Server.Common.Notifications;
using DBI.Server.Features.TreasureSolver.Workers;
using MediatR;

namespace DBI.Server.Features.TreasureSolver;

class TriggerDplnDataSourceRefreshOnNewVersionAvailable(RefreshDplnDataSource dataSource, ILogger<TriggerDplnDataSourceRefreshOnNewVersionAvailable> logger)
    : INotificationHandler<NewVersionAvailableNotification>
{
    public Task Handle(NewVersionAvailableNotification notification, CancellationToken cancellationToken)
    {
        if (notification.NewLatestVersion == notification.OldLatestVersion)
        {
            // We only care about the latest version when refreshing the DPLN data, if it has not changed we don't need to refresh.
            return Task.CompletedTask;
        }

        logger.LogInformation("Latest version has changed, DPLN data source will be refreshed ASAP.");
        dataSource.TriggerRefreshAsap();
        return Task.CompletedTask;
    }
}
