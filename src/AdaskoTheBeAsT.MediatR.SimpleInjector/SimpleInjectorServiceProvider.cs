using System;
using SimpleInjector;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

public class SimpleInjectorServiceProvider
    : IServiceProvider
{
    private readonly Container _container;

    public SimpleInjectorServiceProvider(Container container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public object? GetService(Type serviceType)
    {
        // Return null when the service is not registered, matching Microsoft DI semantics.
        var registration = _container.GetRegistration(serviceType, throwOnFailure: false);
        return registration?.GetInstance();
    }
}
