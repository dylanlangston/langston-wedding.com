namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}