using System.Reflection;
using System.Text.Json;
using CrossCutting.Commands;
using CrossCutting.Configuration;
using CrossCutting.DomainEvents;
using CrossCutting.Queries;
using CrossCutting.Services;
using Domain.SharedKernel;
using Domain.SharedKernel.CQRS;
using Domain.SharedKernel.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CrossCutting.Extensions;
public static class DependencyInjectionExtensions
{
    private delegate void RegisterDIAction(Type serviceType, Type implementationType);
    private static void WithHandler<T, T2>(this System.Reflection.Assembly assembly, RegisterDIAction register) where T : IHandler<T2>
    {
        var exportedTypes = assembly.GetExportedTypes();

        var genericHandlerType = typeof(T).GetGenericTypeDefinition();

        var handlers = exportedTypes
                .Select(t => (Type: t, Interfaces: t.GetInterfaces().Where(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == genericHandlerType
                )))
                .Where(qh => !qh.Type.IsAbstract && qh.Interfaces.Any())
                .ToList();


        handlers.ForEach(h =>
        {
            foreach (var @interface in h.Interfaces)
            {
                register(@interface, h.Type);
            }
        });
    }

    public static void AddDomainEventDispatcher(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        foreach (var assembly in assemblies)
        {
            assembly.WithHandler<IDomainEventHandler<IDomainEvent>, IDomainEvent>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
        }
    }

    public static void AddCQRSDispatchers(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        foreach (var assembly in assemblies)
        {
            assembly.WithHandler<ICommandHandler<ICommand>, ICommand>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
            assembly.WithHandler<IQueryHandler<IQuery>, IQuery>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
        }
    }

    public static void AddServices(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var serviceAttributeType = typeof(ServiceAttribute<>);
            var serviceDetails = assembly.GetExportedTypes()
                .Select(t => (Type: t, ServiceAttributes: t.GetCustomAttributes(serviceAttributeType, false)))
                .Where(t => t.ServiceAttributes.Length != 0)
                .ToList();

            serviceDetails.ForEach(details =>
            {
                foreach ((Type? interfaceType, ServiceRegistrationType? RegistrationType) in details.ServiceAttributes.Select(a => ((Type?)((dynamic?)a)?.InterfaceType, (ServiceRegistrationType?)((dynamic?)a)?.RegistrationType)))
                {
                    if (interfaceType == null) continue;

                    switch (RegistrationType)
                    {
                        case ServiceRegistrationType.Scoped:
                            builder.Services.AddScoped(interfaceType, details.Type);
                            break;
                        case ServiceRegistrationType.Transient:
                            builder.Services.AddTransient(interfaceType, details.Type);
                            break;
                        case ServiceRegistrationType.Singleton:
                            builder.Services.AddSingleton(interfaceType, details.Type);
                            break;
                        default:
                            throw new NotImplementedException($"Registration type: {RegistrationType}");
                    }
                }
            });
        }
    }

    public static void AddConfigurations(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var configurationDetails = assembly.GetExportedTypes()
                        .Select(t => (Type: t, ConfigurationAttributes: t.GetCustomAttributes<ConfigurationAttribute>(false)))
                        .Where(t => t.ConfigurationAttributes.Any())
                        .ToList();

            configurationDetails.ForEach(details =>
            {
                foreach (var key in details.ConfigurationAttributes.Select(a => a.ConfigKey))
                {
                    var configSection = builder.Configuration.GetSection(key);

                    builder.Services.Configure<IConfigurationSection>(configSection);
                    // Call the builder.Services.Configure<T>(configSection) method
                    var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(
                        nameof(OptionsConfigurationServiceCollectionExtensions.Configure),
                        BindingFlags.Static | BindingFlags.Public,
                        [typeof(IServiceCollection), typeof(IConfiguration)]
                        );
                    var configureMethodForType = configureMethod!.MakeGenericMethod(details.Type);
                    configureMethodForType!.Invoke(null, [builder.Services, configSection]);

                    Console.WriteLine(key + " " + details.Type.FullName);

                    builder.Services.AddScoped(details.Type, sp =>
                    {
                        var options = (IOptionsMonitor<object>)sp.GetRequiredService(typeof(IOptionsMonitor<>).MakeGenericType(details.Type));                        
                        return options.CurrentValue;
                    });
                }
            });
        }
    }

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
}