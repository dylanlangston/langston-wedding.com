using System.Text.Json.Serialization;
using Domain.Contact.Aggregates;
using Domain.SharedKernel.DomainEvents;

namespace Domain.Contact.DomainEvents;

public class ContactRequestCreatedEvent : DomainEvent
{
    [JsonInclude]
    ContactRequest Request { get; }

    public ContactRequest GetRequest() => Request;
    
    [JsonInclude]
    public Guid ContactRequestId { get; }

    private ContactRequestCreatedEvent() {}

    public ContactRequestCreatedEvent(
        ContactRequest request)
    {
        Request = request;
        ContactRequestId = Request.Id;
    }
}