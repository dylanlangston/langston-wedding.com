using Domain.SharedKernel.Repositories;

namespace Domain.SharedKernel.DomainEvents;

public interface IDomainEventRepository
    : IAppendRepository<DomainEvent, DateTimeOffset>
    { }