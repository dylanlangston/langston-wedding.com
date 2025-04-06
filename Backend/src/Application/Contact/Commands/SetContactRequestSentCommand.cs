namespace Application.Contact.Commands;

public record SetContactRequestSentCommand(
    Guid Id
) : ICommand;