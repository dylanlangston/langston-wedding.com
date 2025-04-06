using System.IO.Compression;
using Domain.SharedKernel.DomainEvents;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Functions;

public class DurableFunctionsDomainEventQueue : BaseFunction, IDomainEventQueue
{
    internal const string OrchestrationInstanceId = nameof(DurableFunctionsDomainEventQueue);

    DurableTaskClient _durableTaskClient;

    public DurableFunctionsDomainEventQueue(
        ILogger<DurableFunctionsDomainEventQueue> logger,
        DurableTaskClient durableTaskClient,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        JsonSerializerOptions jsonSerializerOptions
        ) : base(logger, commandDispatcher, queryDispatcher, jsonSerializerOptions)
    {
        _durableTaskClient = durableTaskClient;
    }

    [Function(OrchestrationInstanceId)]
    public async Task Orchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context,
        string serializedDomainEvent,
        CancellationToken cancellationToken
        )
    {
        await context.CallActivityAsync<Task>(DurableFunctionsDomainEventWorker.ActivityInstanceId, serializedDomainEvent);
    }

    public async Task EnqueueAsync(DomainEvent domainEvent)
    {
        _logger.LogInformation("Domain event enqueued into the orchestrator.  {EventType}", domainEvent.GetType().Name);

        // Serialization handled by cast
        string serializedDomainEvent = domainEvent;

        await _durableTaskClient.ScheduleNewOrchestrationInstanceAsync(
            OrchestrationInstanceId,
            serializedDomainEvent,
            new StartOrchestrationOptions()
            {
                InstanceId = $"{domainEvent.GetType().FullName}_{domainEvent.OccurredOn.ToString("o")}",
            }
        );

    }
    public Task<DomainEvent?> DequeueAsync()
    {
        throw new NotImplementedException("Items cannot be dequeued as they are processed instantly as durable activity functions.");
    }

    public Task<DomainEvent?> DequeueAsync(CancellationToken _) => DequeueAsync();
}