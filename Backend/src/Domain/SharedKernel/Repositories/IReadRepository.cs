namespace Domain.SharedKernel.Repositories;

public interface IReadRepository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId> where TId : IEquatable<TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<T?> FindAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
}