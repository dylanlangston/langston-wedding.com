namespace Domain.SharedKernel.CQRS;

public interface ICommandHandler<in TCommand> : IHandler<TCommand> where TCommand : ICommand {}

