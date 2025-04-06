using System.Reflection;
using Domain.SharedKernel;
using Domain.SharedKernel.DomainEvents;
using Domain.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossCutting.Extensions;
public static class RepositoriesExtensions
{
    public static void AddDbContext<T, T2>(this IHostApplicationBuilder builder, Action<DbContextOptionsBuilder>? dbContactOptionsConfigure, params System.Reflection.Assembly[] assemblies) where T : DbContext where T2 : class, IUnitOfWork
    {
        builder.Services.AddDbContext<T>(dbContactOptionsConfigure);
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<T>());

        builder.Services.AddScoped<IUnitOfWork, T2>();

        // Register repositories
        foreach (var assembly in assemblies)
        {
            var exportedTypes = assembly.GetExportedTypes();

            var respositoryType = typeof(IRepository<,>).GetGenericTypeDefinition();

            var respositories = exportedTypes
                    .Select(t => (Type: t, Interfaces: t.GetInterfaces().Where(i =>
                        !i.IsGenericType && i.GetInterface(respositoryType.Name)?.GetGenericTypeDefinition() == respositoryType
                    )))
                    .Where(qh => !qh.Type.IsAbstract && qh.Interfaces.Any())
                    .ToList();

            respositories.ForEach(r =>
            {
                foreach (var @interface in r.Interfaces)
                {
                    builder.Services.AddScoped(@interface, r.Type);
                }
            });
        }
    }

    public delegate void ConfigureRepositoryEntity(ModelBuilder modelBuilder, Type entityType, EntityTypeBuilder builder);
    public static void ConfigureRepositories(this IHostApplicationBuilder builder, ConfigureRepositoryEntity configureRepositoryEntity, params System.Reflection.Assembly[] assemblies)
    {
        var entityTypes = new List<Type>();
        foreach (var assembly in assemblies)
        {
            var repositoriesTypes = assembly.GetExportedTypes()
                    .Select(t => (Type: t, Interfaces: t.GetInterfaces().Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IRepository<,>))))
                    .Where(t => !t.Type.IsAbstract && t.Interfaces.Any())
                    .ToList();

            // Add the first arugment (BaseEntity) to the entitesType
            entityTypes.AddRange(
                repositoriesTypes.SelectMany(
                    t => t.Interfaces.Select(
                        i => i.GenericTypeArguments.First())));
        }

        var configureModelBuilder = StartupTask<DbContext, ModelBuilder>.Create((
            DbContext sender,
            params IEnumerable<ModelBuilder?> models
        ) =>
        {
            foreach (var modelBuilder in models)
            {
                if (modelBuilder == null) continue;

                entityTypes.ForEach(repository =>
                {
                    var entityModelBuilder = modelBuilder.Entity(repository);

                    configureRepositoryEntity(modelBuilder, repository, entityModelBuilder);
                });

                #region Domain Events
                var derivedDomainEventTypes = assemblies.SelectMany(
                    a => a.GetExportedTypes().Where(
                        t => t != typeof(DomainEvent) && t.IsAssignableTo(typeof(DomainEvent)))).ToList();

                var domainEventModelBuilder = modelBuilder.Entity<DomainEvent>();
                domainEventModelBuilder
                    .Property(e => e.Id)
                    .HasConversion(new ValueConverter<DateTimeOffset, string>(
                        v => v.ToString("o"), // Converts DateTimeOffset to ISO 8601 string
                        v => DateTimeOffset.Parse(v)
                    ));

                foreach (var derivedType in derivedDomainEventTypes)
                {
                    var derivedEntityModelBuilder = modelBuilder.Entity(derivedType)
                        .HasBaseType(typeof(DomainEvent));
                }
                #endregion
            }
        });
        builder.Services.AddSingleton(configureModelBuilder);
    }
}