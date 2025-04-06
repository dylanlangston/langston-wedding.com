using System.Text.Json.Serialization;
using Domain.Contact.DomainEvents;
using Domain.Contact.ValueObjects;

namespace Domain.Contact.Aggregates;

public enum RequestStatus
{
    Pending,
    Sent
}

public class ContactRequest : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }

    public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;

    [JsonConstructor]
    private ContactRequest(
        Guid Id, 
        string Name, 
        string Email, 
        string Message,
        DateTimeOffset SubmittedAt) : base(Id) {
            this.Name = Name;
            this.Email = Email;
            this.Message = Message;
            this.SubmittedAt = SubmittedAt;
         }
    private ContactRequest() : base(Guid.NewGuid()) { }

    public static Result<ContactRequest> Create(PersonName name, Email email, string message)
    {
        if (string.IsNullOrWhiteSpace(message) || message.Length > 5000)
        {
            return Result.Failure<ContactRequest>("Message must be between 1 and 5000 characters.");
        }

        var contact = new ContactRequest()
        {
            Name = name,
            Email = email,
            Message = message.Trim(),
            SubmittedAt = DateTimeOffset.UtcNow
        };

        // Create Domain Event
        contact.RaiseDomainEvent(new ContactRequestCreatedEvent(contact));

        return contact;
    }
}