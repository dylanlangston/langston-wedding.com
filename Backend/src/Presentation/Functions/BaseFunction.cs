using Microsoft.Extensions.Logging;

namespace Functions;

public abstract class BaseFunction
{
    protected readonly ILogger<BaseFunction> _logger;
    protected readonly ICommandDispatcher _commandDispatcher;
    protected readonly IQueryDispatcher _queryDispatcher;
    protected readonly JsonSerializerOptions _jsonSerializerOptions;

    public BaseFunction(
        ILogger<BaseFunction> logger, 
        ICommandDispatcher commandDispatcher, 
        IQueryDispatcher queryDispatcher,
        JsonSerializerOptions jsonSerializerOptions) {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _jsonSerializerOptions = jsonSerializerOptions;
    }
}