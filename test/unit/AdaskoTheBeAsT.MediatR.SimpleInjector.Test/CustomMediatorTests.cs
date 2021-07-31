using System;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using Moq;
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
        }

        [Fact]
        public void ShouldResolveMediatorWhenCustomMediatorTypeRegistered()
        {
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using<MyCustomMediator>();
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetInstance<IMediator>().Should().NotBeNull();
            _container.GetInstance<IMediator>().GetType().Should().Be(typeof(MyCustomMediator));
        }

        [Fact]
        public void ShouldResolveRequestHandlerWhenCustomMediatorTypeRegistered()
        {
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using<MyCustomMediator>();
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveNotificationHandlersWhenCustomMediatorTypeRegistered()
        {
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using<MyCustomMediator>();
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
        }

        [Fact]
        public void ShouldResolveMediatorWhenCustomMediatorInstanceRegistered()
        {
            var customMediator = new Mock<IMediator>().Object;
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using(() => customMediator);
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetInstance<IMediator>().Should().NotBeNull();
            _container.GetInstance<IMediator>().GetType().Should().Be(customMediator.GetType());
        }

        [Fact]
        public void ShouldResolveRequestHandlerWhenCustomMediatorInstanceRegistered()
        {
            var customMediator = new Mock<IMediator>().Object;
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using(() => customMediator);
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetInstance<IRequestHandler<Ping, Pong>>().Should().NotBeNull();
        }

        [Fact]
        public void ShouldResolveNotificationHandlersWhenCustomMediatorInstanceRegistered()
        {
            var customMediator = new Mock<IMediator>().Object;
            _container.AddMediatR(
                cfg =>
                {
                    cfg.Using(() => customMediator);
                    cfg.WithHandlerAssemblyMarkerTypes(typeof(CustomMediatorTests));
                });
            _container.GetAllInstances<INotificationHandler<Pinged>>().Should().HaveCount(3);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
