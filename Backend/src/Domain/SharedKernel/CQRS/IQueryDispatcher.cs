namespace Domain.SharedKernel.CQRS;

public interface IQueryDispatcher : IDispatcher<IQuery> {}