using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Web;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet.Test
{
    public sealed class ContainerExtensionTest
        : IDisposable
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<HttpContextBase> _httpContextAccessorMock;
        private readonly Container _container;

        public ContainerExtensionTest()
        {
            _cancellationToken = new CancellationTokenSource().Token;
            var httpResponseMock = new Mock<HttpResponseBase>();
            httpResponseMock
                .SetupGet(h => h.ClientDisconnectedToken)
                .Returns(() => _cancellationToken);
            _httpContextAccessorMock = new Mock<HttpContextBase>();
            _httpContextAccessorMock
                .SetupGet(h => h.Response)
                .Returns(httpResponseMock.Object);
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetMediator()
        {
            // Arrange
            _container.AddMediatRAspNet(
                config =>
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object)
                        .WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest)));

            _container.Verify();
            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
            }
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetMediatorWithAssemblyParams()
        {
            // Arrange
            _container.AddMediatRAspNet(
                () => _httpContextAccessorMock.Object,
                typeof(ContainerExtensionTest).GetTypeInfo().Assembly);

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
            }
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetMediatorWithAssemblyEnumerable()
        {
            // Arrange
            _container.AddMediatRAspNet(
                () => _httpContextAccessorMock.Object,
                new List<Assembly> { typeof(ContainerExtensionTest).GetTypeInfo().Assembly });

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
            }
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetMediatorWithHandlerAssemblyMarkerParams()
        {
            // Arrange
            _container.AddMediatRAspNet(() => _httpContextAccessorMock.Object, typeof(ContainerExtensionTest));

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
            }
        }

        [Fact]
        public void ShouldUseDecoratorAfterRegisteringAspNetMediatorWithHandlerAssemblyMarkerEnumerable()
        {
            // Arrange
            _container.AddMediatRAspNet(
                () => _httpContextAccessorMock.Object,
                new List<Type> { typeof(ContainerExtensionTest) });

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
            }
        }

        [Fact]
        public void DecoratorShouldHaveNativeIMediatorImplementation()
        {
            // Arrange
            _container.AddMediatRAspNet(
                config =>
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object).WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest)));

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
                    var innerMediator =
                        GetInstanceFieldValue<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator,
                            IMediator>(
                            result,
                            "_mediator");
                    innerMediator.Should().BeOfType<Mediator>();
                }
            }
        }

        [Fact]
        public void DecoratorShouldHaveFakeIMediatorType()
        {
            // Arrange
            _container.AddMediatRAspNet(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                    config.Using<FakeMediator>();
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object);
                });

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
                    var innerMediator =
                        GetInstanceFieldValue<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator,
                            IMediator>(
                            result,
                            "_mediator");
                    innerMediator.Should().BeOfType<FakeMediator>();
                }
            }
        }

        [Fact]
        public void DecoratorShouldHaveFakeIMediatorInstance()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>().Object;
            _container.AddMediatRAspNet(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                    config.Using(() => mediatorMock);
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object);
                });

            _container.Verify();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var result = _container.GetInstance<IMediator>();

                // Assert
                using (new AssertionScope())
                {
                    result.Should().BeOfType<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator>();
                    var innerMediator =
                        GetInstanceFieldValue<HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator,
                            IMediator>(
                            result,
                            "_mediator");
                    innerMediator.Should().NotBeNull();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    innerMediator.GetType().Should().Be(mediatorMock.GetType());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
        }

        [Fact]
        public void DecoratorShouldPassTypedRequestToInnerMediator()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            IRequest<object>? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            mediatorMock.Setup(
                    m =>
                        m.Send(
                            It.IsAny<IRequest<object>>(),
                            It.IsAny<CancellationToken>()))
                .Callback<IRequest<object>, CancellationToken>(
                    (
                        req,
                        token) =>
                    {
                        savedRequest = req;
                        savedCancellationToken = token;
                    });
            _container.AddMediatRAspNet(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(FakeMediator));
                    config.Using(() => mediatorMock.Object);
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object);
                });

            _container.Verify();

            var request = new Mock<IRequest<object>>().Object;

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var mediator = _container.GetInstance<IMediator>();
                mediator.Send(request, savedCancellationToken);

                // Assert
                using (new AssertionScope())
                {
                    mediatorMock.Verify(
                        m =>
                            m.Send(
                                It.IsAny<IRequest<object>>(),
                                It.IsAny<CancellationToken>()));
                    savedRequest.Should().Be(request);
                    savedCancellationToken.Should().NotBe(_cancellationToken);
                }
            }
        }

        [Fact]
        public void DecoratorShouldPassObjectToInnerMediator()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            object? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            mediatorMock.Setup(
                    m =>
                        m.Send(
                            It.IsAny<object>(),
                            It.IsAny<CancellationToken>()))
                .Callback<object, CancellationToken>(
                    (
                        req,
                        token) =>
                    {
                        savedRequest = req;
                        savedCancellationToken = token;
                    });
            _container.AddMediatRAspNet(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(FakeMediator));
                    config.Using(() => mediatorMock.Object);
                    config.UsingHttpContextCreator(() => _httpContextAccessorMock.Object);
                });

            _container.Verify();

            var request = new object();

            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                // Act
                var mediator = _container.GetInstance<IMediator>();
                mediator.Send(request, savedCancellationToken);

                // Assert
                using (new AssertionScope())
                {
                    mediatorMock.Verify(
                        m =>
                            m.Send(
                                It.IsAny<object>(),
                                It.IsAny<CancellationToken>()));
                    savedRequest.Should().Be(request);
                    savedCancellationToken.Should().NotBe(_cancellationToken);
                }
            }
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        private TResult? GetInstanceFieldValue<T, TResult>(object instance, string fieldName)
            where TResult : class
        {
            const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var type = typeof(T);
            var field = type.GetField(fieldName, bindFlags);
            var value = field?.GetValue(instance);

            return value as TResult;
        }
    }
}
