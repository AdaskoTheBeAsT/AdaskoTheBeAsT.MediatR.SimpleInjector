using System;
using System.Collections.Generic;
using System.Reflection;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;
using static AdaskoTheBeAsT.MediatR.SimpleInjector.Test.PipelineMultiCallToConstructorTests;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class AssemblyResolutionTests
    : IDisposable
{
    private readonly Container _container;

    public AssemblyResolutionTests()
    {
        _container = new Container();
        _container.RegisterSingleton<Logger>();
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
        _container.GetInstance<IRequestHandler<InternalPing, Unit>>().Should().NotBeNull();
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
        _container.GetInstance<IRequestHandler<InternalPing, Unit>>().Should().NotBeNull();
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
