using Domain.Contact.Aggregates;

namespace Domain.Contact.Repositories;

public interface IContactRequestRepository : IRepository<ContactRequest, Guid> {}