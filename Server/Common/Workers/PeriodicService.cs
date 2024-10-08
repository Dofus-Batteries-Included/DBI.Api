﻿namespace DBI.Server.Common.Workers;

abstract class PeriodicService : BackgroundService
{
    readonly TimeSpan _period;
    bool _triggerAsap;

    protected PeriodicService(TimeSpan period, ILogger logger)
    {
        _period = period;
        Logger = logger;
    }

    protected ILogger Logger { get; }

    public void TriggerAsap() => _triggerAsap = true;

    protected override sealed async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await OnStartAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _triggerAsap = false;

            try
            {
                await OnTickAsync(stoppingToken);
            }
            catch (Exception exn)
            {
                Logger.LogError(exn, "Failed to execute periodic task.");
            }

            DateTime start = DateTime.Now;
            while (!stoppingToken.IsCancellationRequested && !_triggerAsap && DateTime.Now - start < _period)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        await OnStopAsync(stoppingToken);
    }

    protected virtual Task OnStartAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    protected virtual Task OnTickAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    protected virtual Task OnStopAsync(CancellationToken stoppingToken) => Task.CompletedTask;
}
