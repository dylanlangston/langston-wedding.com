using Domain.Contact.Aggregates;

namespace Domain.Contact.Specifications
{
    public class RecentContactRequestSpecification : BaseSpecification<ContactRequest>
    {
        public RecentContactRequestSpecification(DateTimeOffset submittedOnOrAfter)
            : base(contact => contact.SubmittedAt >= submittedOnOrAfter)
        {
            ApplyOrderByDescending(contact => contact.SubmittedAt);
        }

        public RecentContactRequestSpecification()
            : base(c => true)
        {
            ApplyOrderByDescending(contact => contact.SubmittedAt);
        }
    }
}