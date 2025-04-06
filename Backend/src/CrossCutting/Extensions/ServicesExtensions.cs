using CrossCutting.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossCutting.Extensions;

public static class ServicesExtensions
{
    public static void AddServices(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var serviceAttributeType = typeof(ServiceAttribute);
            var serviceDetails = assembly.GetExportedTypes()
                .Select(t => (Type: t, ServiceAttributes: t.GetCustomAttributes(serviceAttributeType, false)))
                .Where(t => t.ServiceAttributes.Length != 0)
                .ToList();

            serviceDetails.ForEach(details =>
            {
                if (details.Type == typeof(ServiceAttribute))
                {
                    foreach (ServiceRegistrationType? RegistrationType in details.ServiceAttributes.Select(a => (ServiceRegistrationType?)((dynamic?)a)?.RegistrationType))
                    {
                        switch (RegistrationType)
                        {
                            case ServiceRegistrationType.Scoped:
                                builder.Services.AddScoped(details.Type);
                                break;
                            case ServiceRegistrationType.Transient:
                                builder.Services.AddTransient(details.Type);
                                break;
                            case ServiceRegistrationType.Singleton:
                                builder.Services.AddSingleton(details.Type);
                                break;
                            case ServiceRegistrationType.Background:
                                builder.Services.AddSingleton(details.Type);
                                builder.Services.AddHostedService((provider) => (BackgroundService)provider.GetRequiredService(details.Type));
                                break;
                            default:
                                throw new NotImplementedException($"Registration type: {RegistrationType}");
                        }
                        return;
                    }
                }

                foreach ((Type? interfaceType, ServiceRegistrationType? RegistrationType) in details.ServiceAttributes.Select(a => ((Type?)((dynamic?)a)?.InterfaceType, (ServiceRegistrationType?)((dynamic?)a)?.RegistrationType)))
                {
                    if (interfaceType == null) throw new InvalidOperationException();

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
                        case ServiceRegistrationType.Background:
                            builder.Services.AddSingleton(details.Type);
                            builder.Services.AddHostedService((provider) => (BackgroundService)provider.GetRequiredService(details.Type));
                            break;
                        default:
                            throw new NotImplementedException($"Registration type: {RegistrationType}");
                    }
                }
            });
        }
    }
}