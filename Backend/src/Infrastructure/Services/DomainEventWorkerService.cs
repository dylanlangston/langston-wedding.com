using Domain.SharedKernel.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DomainEventWorkerService : IDomainEventWorker
{
    private readonly IDomainEventQueue _queue;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DomainEventWorkerService> _logger;
    private CancellationTokenSource _cts;
    private Task _workerTask;

    public DomainEventWorkerService(
        IDomainEventQueue queue,
        IDomainEventDispatcher dispatcher,
        IUnitOfWork unitOfWork,
        ILogger<DomainEventWorkerService> logger)
    {
        _queue = queue;
        _dispatcher = dispatcher;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cts = new CancellationTokenSource();
    }

    public Task StartAsync(CancellationToken? cancellationToken = null)
    {
        _logger.LogInformation("DomainEventWorker is starting.");
        var linkedCts = cancellationToken.HasValue ?
            CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cancellationToken.Value) :
            _cts;
        _workerTask = Task.Run(() => ProcessEvents(linkedCts.Token), linkedCts.Token);
        return Task.CompletedTask;
    }

    private async Task ProcessEvents(CancellationToken cancellationToken, bool useCancelationTokenToDequeue = true)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            DomainEvent? domainEvent;
            try
            {
                domainEvent = useCancelationTokenToDequeue ? 
                    await _queue.DequeueAsync(cancellationToken) :
                    await _queue.DequeueAsync();
            }
            catch (OperationCanceledException) { break; }

            if (domainEvent == null)
            {
                break;
            }

            try
            {
                await _dispatcher.Dispatch(domainEvent, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing domain event: {EventType}", domainEvent.GetType().Name);
                Console.WriteLine(ex);
            }
            finally
            {
                await _unitOfWork.SaveChangesAsync(CancellationToken.None);
            }
        }
    }

    public async Task StopAsync()
    {
        _logger.LogInformation("DomainEventWorker is stopping.");

        _cts.Cancel();
        await _workerTask;

        // Process remaning events in the queue
        await ProcessEvents(CancellationToken.None, false);

        _logger.LogInformation("DomainEventWorker is stopped.");
    }
}