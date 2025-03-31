using Domain.SharedKernel.DomainEvents;

namespace Domain.Contact.DomainEvents;

public record ContactRequestCreatedEvent : IDomainEvent
{
    public Guid ContactRequestId { get; }

    public string SubmitterName { get; }
    public string SubmitterEmail { get; }
    public string Message { get; }

    public DateTimeOffset OccurredOn { get; }

    public ContactRequestCreatedEvent(
        Guid contactId,
        string submitterName,
        string submitterEmail,
        string message)
    {
        ContactRequestId = contactId;
        SubmitterName = submitterName;
        SubmitterEmail = submitterEmail;
        Message = message;
        OccurredOn = DateTimeOffset.UtcNow;
    }
}