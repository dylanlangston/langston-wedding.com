using Microsoft.EntityFrameworkCore;
using Domain.Contact.Aggregates;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private StartupTaskRun<DbContext, ModelBuilder>? _modelBuilderStartup;

    public ApplicationDbContext(StartupTask<DbContext, ModelBuilder>? modelBuilderStartup, DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _modelBuilderStartup = modelBuilderStartup;

        // Run the database creation task synchronously
        var created = Database.EnsureCreatedAsync().ConfigureAwait(true).GetAwaiter().GetResult();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_modelBuilderStartup != null) _modelBuilderStartup(this, modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}