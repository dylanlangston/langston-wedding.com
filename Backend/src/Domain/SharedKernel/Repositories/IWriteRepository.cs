namespace Domain.SharedKernel.Repositories;

public interface IWriteRepository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId> where TId : IEquatable<TId>
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
}