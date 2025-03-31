using Microsoft.EntityFrameworkCore;
using Domain.Contact.Aggregates;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<ContactRequest> Contacts { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}