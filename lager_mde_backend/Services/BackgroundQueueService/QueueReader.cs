namespace lager_mde_backend.Services;

public class QueueReader(BackgroundQueueService<int> queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while( await queue.WaitForTheNextReadAsync(stoppingToken))
        {
            var item = await queue.DequeueAsync(stoppingToken);
        }
    }
}