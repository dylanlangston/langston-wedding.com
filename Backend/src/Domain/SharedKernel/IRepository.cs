namespace Domain.SharedKernel;

public interface IRepository<T, TId> where T : BaseEntity<TId> where TId : IEquatable<TId>
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<T?> FindAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
}