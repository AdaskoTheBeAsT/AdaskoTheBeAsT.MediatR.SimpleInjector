using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test
{
    public class HttpRequestAbortedCancellationTokenMediatorDecoratorTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly HttpRequestAbortedCancellationTokenMediatorDecorator _sut;

        public HttpRequestAbortedCancellationTokenMediatorDecoratorTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _cancellationToken = new CancellationTokenSource().Token;
            _mediatorMock = new Mock<IMediator>();
            _sut = new HttpRequestAbortedCancellationTokenMediatorDecorator(
                _mediatorMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task SendRequestShouldUseRequestAbortedFromHttpContext()
        {
            // Arrange
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(
                    () =>
                        new DefaultHttpContext
                        {
                            RequestAborted = _cancellationToken,
                        });

            IRequest<object>? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
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

            var request = new Mock<IRequest<object>>().Object;

            // Act
            await _sut.Send(request, savedCancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Send(
                            It.IsAny<IRequest<object>>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task SendRequestShouldUseCancellationTokenWhenNonExistentHttpContext()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(default(HttpContext));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            IRequest<object>? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
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

            var request = new Mock<IRequest<object>>().Object;

            // Act
            await _sut.Send(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Send(
                            It.IsAny<IRequest<object>>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task SendObjectShouldUseRequestAbortedFromHttpContext()
        {
            // Arrange
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(
                    () =>
                        new DefaultHttpContext
                        {
                            RequestAborted = _cancellationToken,
                        });

            object? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
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

            var request = new object();

            // Act
            await _sut.Send(request, savedCancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Send(
                            It.IsAny<object>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task SendObjectShouldUseCancellationTokenWhenNonExistentHttpContext()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(default(HttpContext));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            object? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
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

            var request = new object();

            // Act
            await _sut.Send(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Send(
                            It.IsAny<object>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task PublishObjectShouldUseRequestAbortedFromHttpContext()
        {
            // Arrange
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(
                    () =>
                        new DefaultHttpContext
                        {
                            RequestAborted = _cancellationToken,
                        });

            object? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
                    m =>
                        m.Publish(
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

            var request = new object();

            // Act
            await _sut.Publish(request, savedCancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Publish(
                            It.IsAny<object>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task PublishObjectShouldUseCancellationTokenWhenNonExistentHttpContext()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(default(HttpContext));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            object? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
                    m =>
                        m.Publish(
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

            var request = new object();

            // Act
            await _sut.Publish(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Publish(
                            It.IsAny<object>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task PublishNotificationShouldUseRequestAbortedFromHttpContext()
        {
            // Arrange
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(
                    () =>
                        new DefaultHttpContext
                        {
                            RequestAborted = _cancellationToken,
                        });

            INotification? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
                    m =>
                        m.Publish(
                            It.IsAny<INotification>(),
                            It.IsAny<CancellationToken>()))
                .Callback<INotification, CancellationToken>(
                    (
                        req,
                        token) =>
                    {
                        savedRequest = req;
                        savedCancellationToken = token;
                    });

            var request = new Mock<INotification>().Object;

            // Act
            await _sut.Publish(request, savedCancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Publish(
                            It.IsAny<INotification>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public async Task PublishNotificationShouldUseCancellationTokenWhenNonExistentHttpContext()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _httpContextAccessorMock
                .SetupGet(h => h.HttpContext)
                .Returns(default(HttpContext));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            INotification? savedRequest = null;
            CancellationToken savedCancellationToken = default;
            _mediatorMock
                .Setup(
                    m =>
                        m.Publish(
                            It.IsAny<INotification>(),
                            It.IsAny<CancellationToken>()))
                .Callback<INotification, CancellationToken>(
                    (
                        req,
                        token) =>
                    {
                        savedRequest = req;
                        savedCancellationToken = token;
                    });

            var request = new Mock<INotification>().Object;

            // Act
            await _sut.Publish(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                _mediatorMock.Verify(
                    m =>
                        m.Publish(
                            It.IsAny<INotification>(),
                            It.IsAny<CancellationToken>()));
                savedRequest.Should().Be(request);
                savedCancellationToken.Should().Be(_cancellationToken);
            }
        }
    }
}
