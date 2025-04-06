using Domain.Contact.DomainEvents;
using Domain.SharedKernel.DomainEvents;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DomainEvents;

public class ContactRequestEmailSentEventHandler : IDomainEventHandler<ContactRequestEmailSentEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ContactRequestEmailSentEventHandler> _logger;

    public ContactRequestEmailSentEventHandler(IEmailService emailService, ILogger<ContactRequestEmailSentEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public Task<Result> Handle(ContactRequestEmailSentEvent notification, CancellationToken cancellationToken)
    {
        var requestId = notification.ContactRequestId;
        _logger.LogInformation("Handling ContactRequestEmailSentEvent for ContactRequestId: {ContactRequestId}", requestId);


        return Result.Success();
    }
}