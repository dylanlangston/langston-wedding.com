using Domain.Contact.Aggregates;

namespace Domain.Contact.Specifications;

public class ContactRequestByEmailSpecification : BaseSpecification<ContactRequest>
{
    public ContactRequestByEmailSpecification(string email) 
        : base(contact => contact.Email.Value == email) { }
}