
using Application.Contact.Queries;
using Domain.Contact.Repositories;

namespace Application.Contact.QueryHandlers;

public class RecentContactRequestQueryHandler : IQueryHandler<RecentContactRequestQuery>
{
    private readonly IContactRequestRepository _contactRepository;

    public RecentContactRequestQueryHandler(IContactRequestRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Result> Handle(RecentContactRequestQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}