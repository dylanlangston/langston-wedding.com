namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEventQueue
{
    Task EnqueueAsync(DomainEvent domainEvent);
    Task<DomainEvent?> DequeueAsync();
    Task<DomainEvent?> DequeueAsync(CancellationToken cancellationToken);
}