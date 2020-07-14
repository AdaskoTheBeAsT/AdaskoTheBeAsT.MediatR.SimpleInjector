using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
    public class CancellationTokenMediatorDecoratorBaseTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<ICancellationTokenAccessor> _cancellationTokenAccessorMock;
        private readonly SampleCancellationTokenMediatorDecorator _sut;

        public CancellationTokenMediatorDecoratorBaseTest()
        {
            _cancellationTokenAccessorMock = new Mock<ICancellationTokenAccessor>();
            _cancellationToken = new CancellationTokenSource().Token;
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
        public async Task SendRequestShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns((CancellationToken?)null);

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
        public async Task SendObjectShouldUseCancellationTokenFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns(_cancellationToken);

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
        public async Task SendObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns((CancellationToken?)null);

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
        public async Task PublishObjectShouldUseCancellationTokenFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns(_cancellationToken);

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
        public async Task PublishObjectShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns((CancellationToken?)null);

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
        public async Task PublishNotificationShouldUseCancellationTokenFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns(_cancellationToken);

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
        public async Task PublishNotificationShouldUseDefaultCancellationTokenWhenNullReturnedFromAccessor()
        {
            // Arrange
            _cancellationTokenAccessorMock.Setup(accessor => accessor.GetToken())
                .Returns((CancellationToken?)null);

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

        internal class SampleCancellationTokenMediatorDecorator
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
