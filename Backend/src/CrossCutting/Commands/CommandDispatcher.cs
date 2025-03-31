using Domain.SharedKernel;
using Domain.SharedKernel.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Commands;

public class CommandDispatcher : ICommandDispatcher {
    public readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<Result> Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.Handle(command, cancellationToken);
    }
}