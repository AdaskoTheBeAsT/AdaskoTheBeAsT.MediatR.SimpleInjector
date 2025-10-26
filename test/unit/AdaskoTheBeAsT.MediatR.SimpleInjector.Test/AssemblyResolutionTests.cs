using System;
using System.Collections.Generic;
using System.Reflection;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using AwesomeAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Xunit;
using Xunit.Abstractions;
using static AdaskoTheBeAsT.MediatR.SimpleInjector.Test.PipelineMultiCallToConstructorTests;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class AssemblyResolutionTests
    : IDisposable
{
    private readonly Container _container;

    public AssemblyResolutionTests(ITestOutputHelper output)
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
    }

    [Fact]
    public void ShouldResolveMediatorWhenParamsPassed()
    {
        _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
        _container.GetInstance<IMediator>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveRequestHandlerWhenParamsPassed()
    {
        _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
        _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveInternalHandlerWhenParamsPassed()
    {
        _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
        _container.GetInstance<IRequestHandler<InternalPing>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveNotificationHandlersWhenParamsPassed()
    {
        _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
        _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
    }

    [Fact]
    public void ShouldResolveStreamHandlersWhenParamsPassed()
    {
        _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
        _container.GetInstance<IStreamRequestHandler<StreamConstructorTestRequest, StreamConstructorTestResponse>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveMediatorWhenIEnumerablePassed()
    {
        _container.AddMediatR(new List<Assembly> { typeof(Ping).GetTypeInfo().Assembly });
        _container.GetInstance<IMediator>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveRequestHandlerWhenIEnumerablePassed()
    {
        _container.AddMediatR(new List<Assembly> { typeof(Ping).GetTypeInfo().Assembly });
        _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveInternalHandlerWhenIEnumerablePassed()
    {
        _container.AddMediatR(new List<Assembly> { typeof(Ping).GetTypeInfo().Assembly });
        _container.GetInstance<IRequestHandler<InternalPing>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveNotificationHandlersWhenIEnumerablePassed()
    {
        _container.AddMediatR(new List<Assembly> { typeof(Ping).GetTypeInfo().Assembly });
        _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
    }

    public void Dispose()
    {
        _container.Dispose();
    }
}
