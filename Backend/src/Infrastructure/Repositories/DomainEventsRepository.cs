using Domain.SharedKernel.DomainEvents;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class DomainEventsRepository : EfRepository<DomainEvent, DateTimeOffset>, IDomainEventRepository
    {
        public DomainEventsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}