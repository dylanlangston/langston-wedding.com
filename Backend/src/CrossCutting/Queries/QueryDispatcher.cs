using Domain.SharedKernel;
using Domain.SharedKernel.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Queries;

public class QueryDispatcher : IQueryDispatcher {
    public readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<Result> Dispatch<TQuery>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery>>();
        return handler.Handle(query, cancellationToken);
    }
}