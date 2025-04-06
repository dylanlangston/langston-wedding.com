using Domain.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly SemaphoreSlim _saveChangesLock = new SemaphoreSlim(1, 1);

    private readonly DbContext _dbContext;
    IDomainEventQueue _domainEventQueue;

    public UnitOfWork(DbContext dbContext, IDomainEventQueue domainEventQueue)
    {
        _dbContext = dbContext;
        _domainEventQueue = domainEventQueue;

        // List to the Save Requested Event to ensure we save outside of DI
        IUnitOfWork.SaveRequestedRaised += (out Task task) => task = SaveChangesAsync();
    }

    public void Dispose()
    {
        // Not needed as the lifetime of the dbContext is managed by DI
        // _dbContext.Dispose();

        GC.SuppressFinalize(this);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = TrackDomainEvents();

        int saveResult = -1;
        bool lockTaken = false;
        try
        {
            await _saveChangesLock.WaitAsync(cancellationToken);
            lockTaken = true;

            saveResult = await _dbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            if (lockTaken) _saveChangesLock.Release();
        }

        // Domain Events are dispatched in background when a unit of work is complete (saved)
        // This ensures they don't block and are processed async 
        domainEvents?.ForEach(async domainEvent => await _domainEventQueue.EnqueueAsync(domainEvent));

        return saveResult;
    }

    private List<DomainEvent>? TrackDomainEvents()
    {
        var entries = _dbContext.ChangeTracker
            .Entries();
        var domainEntities = entries
            .Where(x => x.Entity.GetType().IsAssignableTo(typeof(BaseEntity)) && (x.Entity as BaseEntity)!.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => (x.Entity as BaseEntity)!.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(x => (x.Entity as BaseEntity)!.ClearDomainEvents());

        return domainEvents;
    }
}