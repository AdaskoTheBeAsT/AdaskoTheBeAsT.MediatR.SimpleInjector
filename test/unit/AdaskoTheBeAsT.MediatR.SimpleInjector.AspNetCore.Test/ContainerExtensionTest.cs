using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test;

public sealed class ContainerExtensionTest
    : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Container _container;

    public ContainerExtensionTest()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _httpContextAccessorMock
            .SetupGet(h => h.HttpContext)
            .Returns(
                () =>
                    new DefaultHttpContext
                    {
                        RequestAborted = _cancellationToken,
                    });
        _container = new Container();
    }

    [Fact]
    public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediator()
    {
        // Arrange
        _container.AddMediatRAspNetCore(
            config => config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest)));
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
    }

    [Fact]
    public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediatorWithAssemblyParams()
    {
        // Arrange
        _container.AddMediatRAspNetCore(typeof(ContainerExtensionTest).GetTypeInfo().Assembly);
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
    }

    [Fact]
    public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediatorWithAssemblyEnumerable()
    {
        // Arrange
        _container.AddMediatRAspNetCore(new List<Assembly> { typeof(ContainerExtensionTest).GetTypeInfo().Assembly });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
    }

    [Fact]
    public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediatorWithHandlerAssemblyMarkerParams()
    {
        // Arrange
        _container.AddMediatRAspNetCore(typeof(ContainerExtensionTest));
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
    }

    [Fact]
    public void ShouldUseDecoratorAfterRegisteringAspNetCoreMediatorWithHandlerAssemblyMarkerEnumerable()
    {
        // Arrange
        _container.AddMediatRAspNetCore(new List<Type> { typeof(ContainerExtensionTest) });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
    }

    [Fact]
    public void DecoratorShouldHaveNativeIMediatorImplementation()
    {
        // Arrange
        _container.AddMediatRAspNetCore(
            config =>
                config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest)));
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
            var innerMediator =
                GetInstanceFieldValue<HttpRequestAbortedMediatorDecorator, IMediator>(
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
                config.Using<FakeMediator>();
            });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
            var innerMediator =
                GetInstanceFieldValue<HttpRequestAbortedMediatorDecorator, IMediator>(
                    result,
                    "_mediator");
            innerMediator.Should().BeOfType<FakeMediator>();
        }
    }

    [Fact]
    public void DecoratorShouldHaveFakeIMediatorInstance()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>(MockBehavior.Strict).Object;
        _container.AddMediatRAspNetCore(
            config =>
            {
                config.WithHandlerAssemblyMarkerTypes(typeof(ContainerExtensionTest));
                config.Using(() => mediatorMock);
            });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        // Act
        var result = _container.GetInstance<IMediator>();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<HttpRequestAbortedMediatorDecorator>();
            var innerMediator =
                GetInstanceFieldValue<HttpRequestAbortedMediatorDecorator, IMediator>(
                    result,
                    "_mediator");
            innerMediator.Should().NotBeNull();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            innerMediator.GetType().Should().Be(mediatorMock.GetType());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }

    [Fact]
    public async Task DecoratorShouldPassTypedRequestToInnerMediatorAsync()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
        IRequest<object>? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        mediatorMock.Setup(
                m =>
                    m.Send(
                        It.IsAny<IRequest<object>>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new object())
            .Callback<IRequest<object>, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });
        _container.AddMediatRAspNetCore(
            config =>
            {
                config.WithHandlerAssemblyMarkerTypes(typeof(FakeMediator));
                config.Using(() => mediatorMock.Object);
            });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        var request = new Mock<IRequest<object>>(MockBehavior.Strict).Object;

        // Act
        var mediator = _container.GetInstance<IMediator>();
        await mediator.Send(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            mediatorMock.Verify(
                m =>
                    m.Send(
                        It.IsAny<IRequest<object>>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task DecoratorShouldPassObjectToInnerMediatorAsync()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
        object? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        mediatorMock.Setup(
                m =>
                    m.Send(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new object())
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });
        _container.AddMediatRAspNetCore(
            config =>
            {
                config.WithHandlerAssemblyMarkerTypes(typeof(FakeMediator));
                config.Using(() => mediatorMock.Object);
            });
        _container.RegisterInstance(_httpContextAccessorMock.Object);

        _container.Verify();

        var request = new object();

        // Act
        var mediator = _container.GetInstance<IMediator>();
        await mediator.Send(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            mediatorMock.Verify(
                m =>
                    m.Send(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _container.Dispose();
    }

#pragma warning disable MA0038,S2325 // Make method static
    private TResult? GetInstanceFieldValue<T, TResult>(object instance, string fieldName)
        where TResult : class
    {
        const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        var type = typeof(T);
        var field = type.BaseType?.GetField(fieldName, bindFlags);
        var value = field?.GetValue(instance);

        return value as TResult;
    }
#pragma warning restore MA0038,S2325 // Make method static
}
