using Application.Contact.Commands;
using Domain.SharedKernel.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation.Core;
using Spectre.Console.Cli;
using Spectre.Console;
using Console;


// A simple type registrar to integrate Microsoft DI with Spectre.Console.Cli.

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var cts = new CancellationTokenSource();
        var builder = Host.CreateApplicationBuilder(args);

        // Configure HostBuilder for our app
        builder.ConfigureHostBuilder(options: new()
        {
            ConfigureLogging = true,
            JsonTypeInfos = []
        });

        // // Set up the Spectre.Console CLI with our custom type registrar.
        var registrar = new TypeRegistrar(builder.Services);
        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.UseAssemblyInformationalVersion();

            config.SetExceptionHandler((ex, resolver) =>
                {
                    AnsiConsole.WriteException(ex, ExceptionFormats.Default);
                    return -99;
                });

            config.AddCommand<ContactRequestCommand>("create")
                .WithExample([ "create", "\"Tester, The\"", "test@foobar.com", "\"Lorem Ipsum....\"" ]);
        });

        System.Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Prevent immediate termination
            cts.Cancel();
            AnsiConsole.MarkupLine("[yellow]Cancellation requested...[/]");
        };

        var appTask = app.RunAsync(args);
        return await registrar.Provider!.StartApp(appTask!, cts.Token);
    }
}