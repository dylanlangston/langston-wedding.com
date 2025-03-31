namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEventHandler<in TDomainEvent> : IHandler<TDomainEvent> where TDomainEvent : IDomainEvent {}