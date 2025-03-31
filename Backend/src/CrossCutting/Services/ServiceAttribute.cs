namespace CrossCutting.Services;

public enum ServiceRegistrationType
{
    Scoped,
    Transient,
    Singleton
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class ServiceAttribute<T> : Attribute where T : class
{
    public ServiceAttribute(ServiceRegistrationType registrationType = ServiceRegistrationType.Scoped)
    {
        var @type = typeof(T);
        if (!@type.IsInterface)
        {
            throw new Exception("ServiceAttribute can only be used with interfaces.");
        }
        RegistrationType = registrationType;
        InterfaceType = @type;
    }

    public ServiceRegistrationType RegistrationType { get; private init; }
    public Type InterfaceType { get; private init; }
}