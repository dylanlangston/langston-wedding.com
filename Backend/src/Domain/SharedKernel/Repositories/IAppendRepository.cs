namespace Domain.SharedKernel.Repositories;

public interface IAppendRepository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId> where TId : IEquatable<TId>
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
}