using System;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public sealed class CustomMediatorTests
        : IDisposable
    {
        private readonly Container _container;

        public CustomMediatorTests()
        {
            _container = new Container();
            _container.RegisterSingleton<Logger>();
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using<MyCustomMediator>();
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
        }

        [Fact]
        public void ShouldResolveMediator()
        {
            _container.GetInstance<IMediator>().Should().NotBeNull();
            _container.GetInstance<IMediator>().GetType().Should().Be(typeof(MyCustomMediator));
        }

        [Fact]
        public void ShouldResolveRequestHandler()
        {
            _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
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
