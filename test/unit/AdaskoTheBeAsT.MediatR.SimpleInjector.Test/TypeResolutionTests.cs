using System;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class TypeResolutionTests
    : IDisposable
{
    private readonly Container _container;

    public TypeResolutionTests()
    {
        _container = new Container();
        _container.RegisterInstance(new Logger());
        _container.AddMediatR(typeof(Ping));
    }

    [Fact]
    public void ShouldResolveMediator()
    {
        _container.GetInstance<IMediator>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveRequestHandler()
    {
        _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveVoidRequestHandler()
    {
        _container.GetInstance<IRequestHandler<Ding>>().Should().NotBeNull();
    }

    [Fact]
    public void ShouldResolveNotificationHandlers()
    {
        _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
    }

    [Fact]
    public void ShouldResolveFirstDuplicateHandler()
    {
        _container.GetInstance<IRequestHandler<DuplicateTest, string>>().Should().NotBeNull();
        _container.GetInstance<IRequestHandler<DuplicateTest, string>>()
            .Should().BeAssignableTo<DuplicateHandler1>();
    }

    public void Dispose()
    {
        _container.Dispose();
    }
}
