using System.Diagnostics.CodeAnalysis;
using CrossCutting.DomainEvents;
using Domain.SharedKernel.DomainEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossCutting.Extensions;
public static class DomainEventExtensions
{
    public static void AddDomainEvents<TDomainEventQueue, TDomainEventWorker>(
        this IHostApplicationBuilder builder,
        params System.Reflection.Assembly[] assemblies)
        where TDomainEventQueue : IDomainEventQueue
        where TDomainEventWorker : IDomainEventWorker
    {
        builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        builder.Services.AddSingleton(typeof(IDomainEventQueue), typeof(TDomainEventQueue));
        builder.Services.AddSingleton(typeof(IDomainEventWorker), typeof(TDomainEventWorker));

        foreach (var assembly in assemblies)
        {
            assembly.WithHandler<IDomainEventHandler<DomainEvent>, DomainEvent>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
        }
    }
}