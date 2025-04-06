using System.Text.Json.Serialization;
using Domain.SharedKernel.DomainEvents;

namespace Domain.Contact.DomainEvents;

public class ContactRequestEmailSentEvent : DomainEvent
{
    [JsonInclude]
    public Guid ContactRequestId { get; init; }
    private ContactRequestEmailSentEvent() { }

    public ContactRequestEmailSentEvent(Guid contactRequestId)
    {
        ContactRequestId = contactRequestId;
    }
}