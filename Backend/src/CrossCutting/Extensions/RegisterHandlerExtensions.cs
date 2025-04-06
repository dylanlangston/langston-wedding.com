using Domain.SharedKernel;

namespace CrossCutting.Extensions;
public static class RegisterHandlerExtensions
{
    internal delegate void RegisterDIAction(Type serviceType, Type implementationType);
    internal static void WithHandler<T, T2>(this System.Reflection.Assembly assembly, RegisterDIAction register) where T : IHandler<T2>
    {
        var exportedTypes = assembly.GetExportedTypes();

        var genericHandlerType = typeof(T).GetGenericTypeDefinition();

        var handlers = exportedTypes
                .Select(t => (Type: t, Interfaces: t.GetInterfaces().Where(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == genericHandlerType
                )))
                .Where(qh => !qh.Type.IsAbstract && qh.Interfaces.Any())
                .ToList();


        handlers.ForEach(h =>
        {
            foreach (var @interface in h.Interfaces)
            {
                register(@interface, h.Type);
            }
        });
    }
}