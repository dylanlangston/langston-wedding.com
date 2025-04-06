using CrossCutting.Commands;
using CrossCutting.Queries;
using Domain.SharedKernel.CQRS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossCutting.Extensions;
public static class CQRSExtensions
{
    public static void AddCQRSDispatchers(this IHostApplicationBuilder builder, params System.Reflection.Assembly[] assemblies)
    {
        builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        builder.Services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        foreach (var assembly in assemblies)
        {
            assembly.WithHandler<ICommandHandler<ICommand>, ICommand>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
            assembly.WithHandler<IQueryHandler<IQuery>, IQuery>(
                (service, implementation) => builder.Services.AddScoped(service, implementation));
        }
    }
}