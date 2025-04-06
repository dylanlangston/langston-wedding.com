using CrossCutting.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace CrossCutting.Extensions;
public static class LoggingExtensions
{
    public static void AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddConsole(options =>
            {
                options.FormatterName = nameof(ColorCodedConsoleFormatter);
            })
            .AddConsoleFormatter<ColorCodedConsoleFormatter, ConsoleFormatterOptions>();
        });
    }
}