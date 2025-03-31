using CrossCutting.Services;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

[Service<IEmailService>]
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendContactNotificationAsync(string recipientEmail, string recipientName, string message, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Simulating sending contact notification to {RecipientEmail}", recipientEmail);
        _logger.LogInformation("Submitter: {Name}, Message: {Message}", recipientName, message);

        return Task.CompletedTask;
    }
}
