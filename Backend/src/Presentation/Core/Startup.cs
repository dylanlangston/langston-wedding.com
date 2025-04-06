using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using CrossCutting.Extensions;
using Domain.SharedKernel.DomainEvents;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Presentation.Core;

public class ConfigureHostBuilderOptions
{
    public required bool ConfigureLogging = false;
    public required IJsonTypeInfoResolver[] JsonTypeInfos = [];
}

public static class Startup
{
    public static void ConfigureHostBuilder(
        this IHostApplicationBuilder builder,
        ConfigureHostBuilderOptions options) => 
        ConfigureHostBuilder<InMemoryDomainEventQueue, DomainEventWorkerService>(
            builder,
            options
        );

    public static void ConfigureHostBuilder<TDomainEventQueue, TDomainEventWorker>(
        this IHostApplicationBuilder builder,
        ConfigureHostBuilderOptions options)
        where TDomainEventQueue : IDomainEventQueue
        where TDomainEventWorker : IDomainEventWorker
    {
        // References to our assemblies for DI
        Assembly[] assemblies = [
            Assembly.GetExecutingAssembly(),
            Application.Assembly.Value,
            CrossCutting.Assembly.Value,
            Domain.Assembly.Value,
            Infrastructure.Assembly.Value
        ];

        // Add Logging
        if (options?.ConfigureLogging ?? false) builder.AddLogging();

        // Add Configurations
        builder.AddConfigurations(assemblies);

        // Add JSONOptions with Source Based Serializers
        builder.AddJsonOptions(options?.JsonTypeInfos ?? []);

        // Add appsettings.json file
        builder.Configuration.AddJsonFile("./appsettings.json", false, true);

        // Add Services
        builder.AddServices(assemblies);

        // Add Commands/Queries
        builder.AddCQRSDispatchers(assemblies);

        // Add Domain Events
        builder.AddDomainEvents<TDomainEventQueue, TDomainEventWorker>(assemblies);

        // Add EFCore + Repositories
        builder.AddDbContext<ApplicationDbContext, UnitOfWork>(optionsBuilder =>
        {
#if DEBUG
            optionsBuilder.UseInMemoryDatabase("DevelopmentDB");
#else
            optionsBuilder.UseCosmos(Environment.GetEnvironmentVariable("CosmosDBConnectionString")!, "Database", o =>
            {

            });
#endif
        }, assemblies);

        // Configure Repositories
        builder.ConfigureRepositories((ModelBuilder modelBuilder, Type type, EntityTypeBuilder builder) =>
        {
            var domainEventModelBuilder = modelBuilder.Entity<DomainEvent>();
            domainEventModelBuilder.Property(e => e.Id);

            builder.ToContainer(type.Name);
            // Use the key as the parition key
            var keyProperty = type.GetProperties().SingleOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
            if (keyProperty != null)
            {
                builder.HasPartitionKey(keyProperty.Name);
                builder.Property(keyProperty.Name)
                    .ToJsonProperty(JsonNamingPolicy.CamelCase.ConvertName(keyProperty.Name));
            }
        }, assemblies);
    }

    public static async Task<T> StartApp<T>(this IServiceProvider serviceProvider, Task<T> appTask, CancellationToken cancellation)
    {
        IDomainEventWorker worker = serviceProvider.GetRequiredService<IDomainEventWorker>();
        await worker.StartAsync(cancellation);

        await appTask;

        await worker.StopAsync();

        return await appTask;
    }
}
