using DBI.Server.Common.Notifications;
using DBI.Server.Features.TreasureSolver.Workers;
using MediatR;

namespace DBI.Server.Features.TreasureSolver;

class TriggerDplnDataSourceRefreshOnLatestVersionChanged(RefreshDplnDataSource dataSource, ILogger<TriggerDplnDataSourceRefreshOnLatestVersionChanged> logger)
    : INotificationHandler<LatestVersionChangedNotification>
{
    public Task Handle(LatestVersionChangedNotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Latest version has changed, DPLN data source will be refreshed ASAP.");
        dataSource.Refresh();
        return Task.CompletedTask;
    }
}
