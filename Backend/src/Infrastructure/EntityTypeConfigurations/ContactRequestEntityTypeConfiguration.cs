using Domain.Contact.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfigurations;

public class ContactRequestEntityTypeConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Email)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(c => c.Message)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(c => c.SubmittedAt)
            .IsRequired();
    }
}