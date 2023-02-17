using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class CancellationTokenMediatorDecoratorBaseTest
    : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ICancellationTokenAccessor> _cancellationTokenAccessorMock;
    private readonly SampleCancellationTokenMediatorDecorator _sut;

    public CancellationTokenMediatorDecoratorBaseTest()
    {
        _cancellationTokenAccessorMock = new Mock<ICancellationTokenAccessor>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _mediatorMock = new Mock<IMediator>();
        _sut = new SampleCancellationTokenMediatorDecorator(
            _mediatorMock.Object,
            _cancellationTokenAccessorMock.Object);
    }

#pragma warning disable CA1034 // Nested types should not be visible
    public interface ICancellationTokenAccessor
    {
        CancellationToken? GetToken();
    }
#pragma warning restore CA1034 // Nested types should not be visible

    [Fact]
    public async Task CreateStreamObjectShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        object? savedRequest = null;
        var responseItem = new object();
        var response = new[] { responseItem }.ToAsyncEnumerable();
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.CreateStream(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Returns(response)
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<object>().Object;

        // Act
        object? result = null;
        await foreach (var o in _sut.CreateStream(request, savedCancellationToken))
        {
            result = o;
        }

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.CreateStream(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
            result.Should().Be(responseItem);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task CreateStreamObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        object? savedRequest = null;
        var responseItem = new object();
        var response = new[] { responseItem }.ToAsyncEnumerable();
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.CreateStream(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Returns(response)
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<object>().Object;

        // Act
        object? result = null;
        await foreach (var o in _sut.CreateStream(request, _cancellationToken))
        {
            result = o;
        }

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.CreateStream(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
            result.Should().Be(responseItem);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task CreateStreamIStreamRequestShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        IStreamRequest<object>? savedRequest = null;
        var responseItem = new object();
        var response = new[] { responseItem }.ToAsyncEnumerable();
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.CreateStream(
                        It.IsAny<IStreamRequest<object>>(),
                        It.IsAny<CancellationToken>()))
            .Returns(response)
            .Callback<IStreamRequest<object>, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<IStreamRequest<object>>().Object;

        // Act
        object? result = null;
        await foreach (var o in _sut.CreateStream(request, savedCancellationToken))
        {
            result = o;
        }

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.CreateStream(
                        It.IsAny<IStreamRequest<object>>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
            result.Should().Be(responseItem);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task CreateStreamIStreamRequestShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        IStreamRequest<object>? savedRequest = null;
        var responseItem = new object();
        var response = new[] { responseItem }.ToAsyncEnumerable();
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.CreateStream(
                        It.IsAny<IStreamRequest<object>>(),
                        It.IsAny<CancellationToken>()))
            .Returns(response)
            .Callback<IStreamRequest<object>, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<IStreamRequest<object>>().Object;

        // Act
        object? result = null;
        await foreach (var o in _sut.CreateStream(request, _cancellationToken))
        {
            result = o;
        }

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.CreateStream(
                        It.IsAny<IStreamRequest<object>>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
            result.Should().Be(responseItem);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task SendRequestShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        IRequest<object>? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<IRequest<object>>(),
                        It.IsAny<CancellationToken>()))
            .Callback<IRequest<object>, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<IRequest<object>>().Object;

        // Act
        await _sut.Send(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
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
    public async Task SendRequestShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        IRequest<object>? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<IRequest<object>>(),
                        It.IsAny<CancellationToken>()))
            .Callback<IRequest<object>, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<IRequest<object>>().Object;

        // Act
        await _sut.Send(request, _cancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
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
    public async Task SendRequestSimpleShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        Ding? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<Ding>(),
                        It.IsAny<CancellationToken>()))
            .Callback<Ding, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Ding();

        // Act
        await _sut.Send(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Send(
                        It.IsAny<Ding>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task SendRequestSimpleShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        Ding? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<Ding>(),
                        It.IsAny<CancellationToken>()))
            .Callback<Ding, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Ding();

        // Act
        await _sut.Send(request, _cancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Send(
                        It.IsAny<Ding>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task SendObjectShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        object? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new object();

        // Act
        await _sut.Send(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
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

    [Fact]
    public async Task SendObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        object? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Send(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new object();

        // Act
        await _sut.Send(request, _cancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
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

    [Fact]
    public async Task PublishObjectShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        object? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Publish(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new object();

        // Act
        await _sut.Publish(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Publish(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task PublishObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        object? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Publish(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new object();

        // Act
        await _sut.Publish(request, _cancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Publish(
                        It.IsAny<object>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task PublishNotificationShouldUseCancellationTokenFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns(_cancellationToken);

        INotification? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Publish(
                        It.IsAny<INotification>(),
                        It.IsAny<CancellationToken>()))
            .Callback<INotification, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<INotification>().Object;

        // Act
        await _sut.Publish(request, savedCancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Publish(
                        It.IsAny<INotification>(),
                        It.IsAny<CancellationToken>()));
#pragma warning restore MA0100

            savedRequest.Should().Be(request);
            savedCancellationToken.Should().Be(_cancellationToken);
        }
#pragma warning restore IDISP013 // Await in using.
    }

    [Fact]
    public async Task PublishNotificationShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessorAsync()
    {
        // Arrange
        _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
            .Returns((CancellationToken?)null);

        INotification? savedRequest = null;
        var savedCancellationToken = default(CancellationToken);
        _mediatorMock
            .Setup(
                m =>
                    m.Publish(
                        It.IsAny<INotification>(),
                        It.IsAny<CancellationToken>()))
            .Callback<INotification, CancellationToken>((req, token) =>
            {
                savedRequest = req;
                savedCancellationToken = token;
            });

        var request = new Mock<INotification>().Object;

        // Act
        await _sut.Publish(request, _cancellationToken);

        // Assert
#pragma warning disable IDISP013 // Await in using.
        using (new AssertionScope())
        {
#pragma warning disable MA0100
            _mediatorMock.Verify(
                m =>
                    m.Publish(
                        It.IsAny<INotification>(),
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
    }

    internal sealed class SampleCancellationTokenMediatorDecorator
        : CancellationTokenMediatorDecoratorBase
    {
        private readonly ICancellationTokenAccessor _cancellationTokenAccessor;

        public SampleCancellationTokenMediatorDecorator(
            IMediator mediator,
            ICancellationTokenAccessor cancellationTokenAccessor)
            : base(mediator)
        {
            _cancellationTokenAccessor = cancellationTokenAccessor;
        }

        public override CancellationToken GetCustomOrDefaultCancellationToken(CancellationToken cancellationToken)
        {
            return _cancellationTokenAccessor.GetToken() ?? cancellationToken;
        }
    }
}
