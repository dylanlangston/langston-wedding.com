namespace Domain.SharedKernel.CQRS;

public interface ICommandDispatcher : IDispatcher<ICommand> {}