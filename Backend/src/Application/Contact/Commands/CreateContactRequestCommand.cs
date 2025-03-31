namespace Application.Contact.Commands;

public record CreateContactRequestCommand(
    string Name,
    string Email,
    string Message
) : ICommand;