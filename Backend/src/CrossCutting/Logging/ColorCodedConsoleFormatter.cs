using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Pastel;

namespace CrossCutting.Logging;

enum Category
{
    Domain,
    Application,
    CrossCutting,
    Infrastructure,
    Internal,
    External
}

public class ColorCodedConsoleFormatter : ConsoleFormatter
{
    public ColorCodedConsoleFormatter() : base(nameof(ColorCodedConsoleFormatter)) { }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        ConsoleColor levelColor = logEntry.LogLevel switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Green,
            LogLevel.Information => ConsoleColor.Blue,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Critical => ConsoleColor.DarkRed,
            _ => ConsoleColor.DarkGray
        };
        var level = $"{logEntry.LogLevel}".Pastel(levelColor);

        ConsoleColor categoryColor = logEntry.Category.Split('.') switch
        {
            ["Domain", ..] => ConsoleColor.Green,
            ["Presentation", ..] => ConsoleColor.Yellow,
            ["Application", ..] => ConsoleColor.Blue,
            ["CrossCutting", ..] => ConsoleColor.Red,
            ["Infrastructure", ..] => ConsoleColor.Magenta,
            ["Functions", ..] or ["Program", ..] => ConsoleColor.Blue,
            _ => ConsoleColor.Gray
        };
        var category = $"{logEntry.Category}".Pastel(categoryColor);

        string message = $"{level}: {category}{Environment.NewLine}\t{logEntry.Formatter(logEntry.State, logEntry.Exception)}";
        textWriter.WriteLine(message);
    }
}
