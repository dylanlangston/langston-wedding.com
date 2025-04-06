public interface IDomainEventWorker
{
    Task StartAsync(CancellationToken? cancellationToken = null);
    Task StopAsync();
}