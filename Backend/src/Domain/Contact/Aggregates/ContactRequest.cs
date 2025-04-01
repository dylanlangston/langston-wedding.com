using Domain.Contact.DomainEvents;
using Domain.Contact.ValueObjects;

namespace Domain.Contact.Aggregates;

public class ContactRequest : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Message { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }

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
        contact.RaiseDomainEvent(new ContactRequestCreatedEvent(
            contact.Id,
            contact.Email,
            contact.Email,
            contact.Message));

        return contact;
    }
}