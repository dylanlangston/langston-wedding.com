using Domain.Contact.Aggregates;
using Domain.Contact.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class ContactRequestRepository : EfRepository<ContactRequest, Guid>, IContactRequestRepository
    {
        public ContactRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}