using Domain.Contact.DomainEvents;
using Domain.SharedKernel.DomainEvents;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DomainEvents;

public class ContactRequestCreatedEventHandler : IDomainEventHandler<ContactRequestCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactRequestCreatedEventHandler> _logger;

    public ContactRequestCreatedEventHandler(IEmailService emailService, ILogger<ContactRequestCreatedEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(ContactRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling ContactRequestCreatedEvent for ContactRequestId: {ContactRequestId}", notification.ContactRequestId);

        try
        {
            await _emailService.SendContactNotificationAsync(
                notification.SubmitterEmail,
                notification.SubmitterName,
                notification.Message,
                cancellationToken);

            _logger.LogInformation("Contact notification email sent successfully for ContactId: {ContactRequestId}", notification.ContactRequestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contact notification email for ContactId: {ContactRequestId}", notification.ContactRequestId);
            return Result.Failure(ex.Message);
        }

        return Result.Success();
    }
}