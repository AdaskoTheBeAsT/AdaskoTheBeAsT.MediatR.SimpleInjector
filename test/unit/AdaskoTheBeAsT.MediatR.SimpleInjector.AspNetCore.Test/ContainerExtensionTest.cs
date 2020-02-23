using System;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test
{
    public sealed class ContainerExtensionTest
        : IDisposable
    {
        private readonly Container _container;

        public ContainerExtensionTest()
        {
            _container = new Container();
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediator()
        {
            // Arrange
            _container.AddMediatRAspNetCore(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                });
            _container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _container.Verify();

            // Act
            var result = _container.GetInstance<IMediator>();

            // Assert
            result.Should().BeOfType<HttpRequestAbortedCancellationTokenMediatorDecorator>();
        }

        [Fact]
        public void DecoratorShouldHaveNativeIMediatorImplementation()
        {
            // Arrange
            _container.AddMediatRAspNetCore(
                config =>
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest)));
            _container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _container.Verify();

            // Act
            var result = _container.GetInstance<IMediator>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<HttpRequestAbortedCancellationTokenMediatorDecorator>();
                var innerMediator =
                    GetInstanceFieldValue<HttpRequestAbortedCancellationTokenMediatorDecorator, IMediator>(
                        result,
                        "_mediator");
                innerMediator.Should().BeOfType<Mediator>();
            }
        }

        [Fact]
        public void DecoratorShouldHaveFakeIMediatorType()
        {
            // Arrange
            _container.AddMediatRAspNetCore(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                    config.Using<FakeMediator<object>>();
                });
            _container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _container.Verify();

            // Act
            var result = _container.GetInstance<IMediator>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<HttpRequestAbortedCancellationTokenMediatorDecorator>();
                var innerMediator =
                    GetInstanceFieldValue<HttpRequestAbortedCancellationTokenMediatorDecorator, IMediator>(
                        result,
                        "_mediator");
                innerMediator.Should().BeOfType<FakeMediator<object>>();
            }
        }

        [Fact]
        public void DecoratorShouldHaveFakeIMediatorInstance()
        {
            // Arrange
            _container.AddMediatRAspNetCore(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                    config.Using<FakeMediator<object>>();
                });
            _container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _container.Verify();

            // Act
            var result = _container.GetInstance<IMediator>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<HttpRequestAbortedCancellationTokenMediatorDecorator>();
                var innerMediator =
                    GetInstanceFieldValue<HttpRequestAbortedCancellationTokenMediatorDecorator, IMediator>(
                        result,
                        "_mediator");
                innerMediator.Should().BeOfType<FakeMediator<object>>();
            }
        }

        [Fact]
        public void DecoratorShouldPassRequestToInnerMediator()
        {
            // Arrange
            _container.AddMediatRAspNetCore(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(FakeMediator<>));
                    config.Using<FakeMediator<object>>();
                });
            _container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _container.Verify();

            // Act
            var result = _container.GetInstance<IMediator>();

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<HttpRequestAbortedCancellationTokenMediatorDecorator>();
                var innerMediator =
                    GetInstanceFieldValue<HttpRequestAbortedCancellationTokenMediatorDecorator, IMediator>(
                        result,
                        "_mediator");
                innerMediator.Should().BeOfType<FakeMediator<object>>();
            }
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        private TResult? GetInstanceFieldValue<T, TResult>(object instance, string fieldName)
            where TResult : class
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var type = typeof(T);
            var field = type.GetField(fieldName, bindFlags);
            var value = field?.GetValue(instance);

            return value as TResult;
        }
    }
}
