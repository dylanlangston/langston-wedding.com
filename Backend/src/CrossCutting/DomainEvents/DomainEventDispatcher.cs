using System.Reflection;
using Domain.SharedKernel;
using Domain.SharedKernel.DomainEvents;

namespace CrossCutting.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    public readonly IServiceProvider _serviceProvider;
    public readonly IDomainEventRepository _domainEventRepository;

    public DomainEventDispatcher(
        IServiceProvider serviceProvider,
        IDomainEventRepository domainEventRepository) {
             _serviceProvider = serviceProvider;
             _domainEventRepository = domainEventRepository;
        }

    public async Task<Result> Dispatch<TEvent>(TEvent notification, CancellationToken cancellationToken) where TEvent : DomainEvent
    {
        var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(notification.GetType());
        var handler = _serviceProvider.GetService(domainEventHandlerType);

        if (handler == null) {
            return Result.Failure("Failed to find an associated event handler");
        }

        var handleMethod = handler.GetType().GetMethod(
            nameof(IDomainEventHandler<TEvent>.Handle),
            BindingFlags.Instance | BindingFlags.Public);

        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handler does not contain a Handle method. {nameof(IDomainEventHandler<TEvent>.Handle)}");
        }

        var result = (Task<Result>)handleMethod.Invoke(handler, [notification, cancellationToken])!;

        await _domainEventRepository.AddAsync(notification, cancellationToken);

        return await result;
    }
}