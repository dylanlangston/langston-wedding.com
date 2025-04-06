using Domain.Contact.Aggregates;
using Domain.SharedKernel.Repositories;

namespace Domain.Contact.Repositories;

public interface IContactRequestRepository
    : IReadRepository<ContactRequest, Guid>, IWriteRepository<ContactRequest, Guid>
{ }