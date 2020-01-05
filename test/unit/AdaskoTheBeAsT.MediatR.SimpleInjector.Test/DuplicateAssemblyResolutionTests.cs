using System;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public sealed class DuplicateAssemblyResolutionTests
        : IDisposable
    {
        private readonly Container _container;

        public DuplicateAssemblyResolutionTests()
        {
            _container = new Container();
            _container.RegisterSingleton<Logger>();
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
}
