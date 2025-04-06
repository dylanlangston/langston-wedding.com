using Application.Contact.Commands;
using CrossCutting.Services;
using Domain.SharedKernel.CQRS;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

[Service<IEmailService>]
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public EmailService(ILogger<EmailService> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    public async Task SendContactNotificationAsync(
        Guid contactRequestId,
        string senderEmail, 
        string senderName, 
        string message, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Simulating sending contact notification on behalf of {RecipientEmail}", senderEmail);
        _logger.LogInformation("Submitter: {Name}, Message: {Message}", senderName, message);

        // Simulate a long running task.
        await Task.Delay(2000);

        await _commandDispatcher.Dispatch(new SetContactRequestSentCommand(contactRequestId));
    }
}
