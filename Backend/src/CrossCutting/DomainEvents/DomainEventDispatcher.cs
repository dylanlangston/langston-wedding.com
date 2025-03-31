using System.Reflection;
using Domain.SharedKernel;
using Domain.SharedKernel.DomainEvents;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    public readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public Task<Result> Dispatch<TEvent>(TEvent notification, CancellationToken cancellationToken) where TEvent : IDomainEvent
    {
        var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(notification.GetType());
        Console.WriteLine(domainEventHandlerType.FullName);
        var handler = _serviceProvider.GetRequiredService(domainEventHandlerType);
        var handleMethod = handler.GetType().GetMethod(
            nameof(IDomainEventHandler<TEvent>.Handle),
            BindingFlags.Instance | BindingFlags.Public);

        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handler does not contain a Handle method. {nameof(IDomainEventHandler<TEvent>.Handle)}");
        }

        return (Task<Result>)handleMethod.Invoke(handler, [notification, cancellationToken])!;
    }
}