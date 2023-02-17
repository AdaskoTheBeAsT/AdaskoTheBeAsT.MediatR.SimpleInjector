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

    public object GetService(Type serviceType) => _container.GetInstance(serviceType);
}
