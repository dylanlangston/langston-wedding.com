using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Function.Configuration;

public static class ConfigurationExtensions {

    public static void AddConfigurationsFromAssembly(this IHostApplicationBuilder builder, Assembly assembly) {
        // Find all Types with the ConfigurationAttribute applied
        var configurationDetails = assembly.GetExportedTypes()
            .Select(t => (Type: t, ConfigurationAttributes: t.GetCustomAttributes<ConfigurationAttribute>(false)))
            .Where(t => t.ConfigurationAttributes.Any())
            .ToList();

        configurationDetails.ForEach(details => {
            foreach (var key in details.ConfigurationAttributes.Select(a => a.ConfigKey)) {
                var configSection = builder.Configuration.GetSection(key);

                builder.Services.Configure<CorsConfiguration>(configSection);
                // Call the builder.Services.Configure<T>(configSection) method
                var configureMethod = typeof(OptionsServiceCollectionExtensions).GetMethod(
                    nameof(OptionsServiceCollectionExtensions.Configure),
                    BindingFlags.Static | BindingFlags.Public, 
                    [typeof(IServiceCollection), typeof(IConfigurationSection)]
                    );
                var configureMethodForType = configureMethod?.MakeGenericMethod(details.Type);
                configureMethodForType?.Invoke(null, [builder.Services, configSection]);

                builder.Services.AddScoped(details.Type, sp => {
                    var options = (IOptionsMonitor<object>)sp.GetRequiredService(typeof(IOptionsMonitor<>).MakeGenericType(details.Type));
                    return options.CurrentValue;
                });
            }
        });
    }
}