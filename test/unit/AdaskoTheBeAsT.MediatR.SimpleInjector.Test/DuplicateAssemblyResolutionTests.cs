using System;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using AwesomeAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Xunit;
using Xunit.Abstractions;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class DuplicateAssemblyResolutionTests
    : IDisposable
{
    private readonly Container _container;

    public DuplicateAssemblyResolutionTests(ITestOutputHelper output)
    {
        _container = new Container();
        _container.RegisterSingleton<Logger>();
        _container.RegisterInstance(output);

        // ILoggerFactory that writes to test output
        _container.RegisterSingleton<ILoggerFactory>(() =>
            LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
#pragma warning disable IDISP004
                builder.AddProvider(new XunitTestOutputLoggerProvider(output));
#pragma warning restore IDISP004
                builder.SetMinimumLevel(LogLevel.Trace);
            }));

        // Wire up ILogger<T> using the factory
        _container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Singleton);
        _container.AddMediatR(typeof(Ping), typeof(Ping));
    }

    [Fact]
    public void ShouldResolveNotificationHandlersOnlyOnce()
    {
        _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
    }

    public void Dispose()
    {
        _container.Dispose();
    }
}
