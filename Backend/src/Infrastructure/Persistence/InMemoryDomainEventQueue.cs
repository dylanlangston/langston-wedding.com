using System.Collections.Concurrent;
using Domain.SharedKernel.DomainEvents;

namespace Infrastructure.Persistence;

public class InMemoryDomainEventQueue : IDomainEventQueue
{
    private readonly ConcurrentQueue<DomainEvent> _eventQueue = new();
    private readonly SemaphoreSlim _queueSignal = new(0);

    public Task EnqueueAsync(DomainEvent domainEvent)
    {
        _eventQueue.Enqueue(domainEvent);
        _queueSignal.Release();

        return Task.CompletedTask;
    }

    public async Task<DomainEvent?> DequeueAsync()
    {
        if (await _queueSignal.WaitAsync(0) && _eventQueue.TryDequeue(out var domainEvent))
        {
            return domainEvent;
        }
        return null;
    }

    public async Task<DomainEvent?> DequeueAsync(CancellationToken cancellationToken)
    {
        await _queueSignal.WaitAsync(cancellationToken);
        if (_eventQueue.TryDequeue(out var domainEvent))
        {
            return domainEvent;
        }
        return null;
    }
}