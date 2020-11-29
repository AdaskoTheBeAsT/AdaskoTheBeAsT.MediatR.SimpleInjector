using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
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
        public async Task SendRequestShouldUseCancellationTokenFromAccessor()
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
        public async Task SendRequestShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
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
        public async Task SendObjectShouldUseCancellationTokenFromAccessor()
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
        public async Task SendObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
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
        public async Task PublishObjectShouldUseCancellationTokenFromAccessor()
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
        public async Task PublishObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
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
        public async Task PublishNotificationShouldUseCancellationTokenFromAccessor()
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
        public async Task PublishNotificationShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
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
}
