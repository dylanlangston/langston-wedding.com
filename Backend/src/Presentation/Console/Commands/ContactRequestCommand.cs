using System.ComponentModel;
using Application.Contact.Commands;
using Domain.SharedKernel.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Console;

[Description("Creates a new contact")]
internal sealed class ContactRequestCommand : AsyncCommand<ContactRequestCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(1, "[name]")]
        [Description("Name of the contact")]
        public string Name { get; init; }

        [CommandArgument(2, "[email]")]
        [Description("Email address of the contact")]
        public string Email { get; init; }

        [CommandArgument(3, "[message]")]
        [Description("Message for the contact request")]
        public string Message { get; init; }
    }

    private readonly IServiceProvider _serviceProvider;

    public ContactRequestCommand(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        using var scope = _serviceProvider.CreateScope();
        var commandDispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ContactRequestCommand>>();

        try
        {
            // Dispatch the CQRS command asynchronously with the provided settings.
            var result = await commandDispatcher.Dispatch(
                new CreateContactRequestCommand(settings.Name, settings.Email, settings.Message));

            if (result.IsFailure)
            {
                throw new Exception(result.Error);
            }

            logger.LogInformation("Contact created successfully.");
            AnsiConsole.MarkupLine("[green]Contact created successfully![/]");

            return 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating contact.");
            AnsiConsole.MarkupLine("[red]Error creating contact:[/]");
            AnsiConsole.WriteException(ex, ExceptionFormats.Default);
            return -1;
        }
    }
}