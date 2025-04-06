using System.IO.Compression;
using Domain.SharedKernel.DomainEvents;
using Microsoft.Extensions.Logging;

namespace Functions;

// This currently does nothing as all DomainEvent Process
public class DurableFunctionsDomainEventWorker : BaseFunction, IDomainEventWorker
{
    internal const string ActivityInstanceId = nameof(DurableFunctionsDomainEventWorker);

    IDomainEventDispatcher _domainEventDispatcher;

    public DurableFunctionsDomainEventWorker(
        ILogger<DurableFunctionsDomainEventWorker> logger,
        IDomainEventDispatcher domainEventDispatcher,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        JsonSerializerOptions jsonSerializerOptions
        ) : base(logger, commandDispatcher, queryDispatcher, jsonSerializerOptions)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public Task StartAsync(CancellationToken? cancellationToken = null)
    {
        // Intentionally left blank
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        // Intentionally left blank
        return Task.CompletedTask;
    }


    [Function(ActivityInstanceId)]
    public async Task Activity(
    [ActivityTrigger] string serializedDomainEvent,
    CancellationToken cancellationToken)
    {
        try
        {
            // Deserialzation handled by cast
            DomainEvent? domainEvent = serializedDomainEvent;

            await _domainEventDispatcher.Dispatch(domainEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while dispatching domain event: {EventType}", serializedDomainEvent);
            throw;
        }
        finally
        {
            await IUnitOfWork.RequestSave();
        }
    }
}