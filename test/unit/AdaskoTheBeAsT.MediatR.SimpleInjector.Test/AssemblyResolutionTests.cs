using System;
using System.Reflection;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public sealed class AssemblyResolutionTests
        : IDisposable
    {
        private readonly Container _container;

        public AssemblyResolutionTests()
        {
            _container = new Container();
            _container.RegisterSingleton<Logger>();
            _container.AddMediatR(typeof(Ping).GetTypeInfo().Assembly);
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
        public void ShouldResolveInternalHandler()
        {
            _container.GetInstance<IRequestHandler<InternalPing, Unit>>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveNotificationHandlers()
        {
            _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
