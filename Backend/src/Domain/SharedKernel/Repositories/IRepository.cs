namespace Domain.SharedKernel.Repositories;

public interface IRepository<T, TId> where T : BaseEntity<TId> where TId : IEquatable<TId>
{
}