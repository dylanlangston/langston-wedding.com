namespace CrossCutting.Services;

public enum ServiceRegistrationType
{
    Scoped,
    Transient,
    Singleton,
    Background
}

public class ServiceAttribute : Attribute
{
    public ServiceAttribute(ServiceRegistrationType registrationType = ServiceRegistrationType.Scoped)
    {
        RegistrationType = registrationType;
    }

    public ServiceRegistrationType RegistrationType { get; private init; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class ServiceAttribute<T> : ServiceAttribute where T : class
{
    public ServiceAttribute(ServiceRegistrationType registrationType = ServiceRegistrationType.Scoped) : base(registrationType)
    {
        var @type = typeof(T);
        InterfaceType = @type;
    }

    public Type InterfaceType { get; private init; }
}