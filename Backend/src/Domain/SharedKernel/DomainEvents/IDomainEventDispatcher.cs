namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEventDispatcher : IDispatcher<DomainEvent> {}