
using Application.Contact.Queries;
using Domain.Contact.Repositories;

namespace Application.Contact.QueryHandlers;

public class ContactRequestByEmailQueryHandler : IQueryHandler<ContactRequestByEmailQuery>
{
    private readonly IContactRequestRepository _contactRepository;

    public ContactRequestByEmailQueryHandler(IContactRequestRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Result> Handle(ContactRequestByEmailQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}