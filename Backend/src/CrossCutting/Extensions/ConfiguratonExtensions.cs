using System.Reflection;
using CrossCutting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CrossCutting.Extensions;
public static class ConfiguratonExtensions
{
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

                    // Call the builder.Services.Configure<T>(configSection) method
                    var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(
                        nameof(OptionsConfigurationServiceCollectionExtensions.Configure),
                        BindingFlags.Static | BindingFlags.Public,
                        [typeof(IServiceCollection), typeof(IConfiguration)]
                        );
                    var configureMethodForType = configureMethod!.MakeGenericMethod(details.Type);
                    configureMethodForType!.Invoke(null, [builder.Services, configSection]);

                    builder.Services.AddScoped(details.Type, sp =>
                    {
                        var options = (IOptionsMonitor<object>)sp.GetRequiredService(typeof(IOptionsMonitor<>).MakeGenericType(details.Type));
                        return options.CurrentValue;
                    });
                }
            });
        }
    }
}