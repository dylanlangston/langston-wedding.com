namespace Domain.SharedKernel.CQRS;

public interface IQueryHandler<in TQuery> : IHandler<TQuery> where TQuery : IQuery {}