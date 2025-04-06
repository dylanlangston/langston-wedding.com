using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Console;

public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    public TypeRegistrar(IServiceCollection builder) => _builder = builder;

    private TypeResolver? _typeResolver;
    public ITypeResolver Build()
    {
        if (_typeResolver == null)
        {
            _typeResolver = new TypeResolver(_builder.BuildServiceProvider());
        }

        return _typeResolver;
    }

    public IServiceProvider? Provider {get => _typeResolver?.Provider; }

    public void Register(Type service, Type implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _builder.AddSingleton(service, factory);
    }
}

public sealed class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _provider;
    public IServiceProvider Provider { get => _provider; }
    public TypeResolver(IServiceProvider provider) => _provider = provider;
    public object? Resolve(Type? type) => type == null ? null : _provider.GetService(type);
}
