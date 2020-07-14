using System.Threading;
using System.Web;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet.Test
{
    public class HttpResponseClientDisconnectedTokenMediatorDecoratorTest
    {
        private readonly CancellationToken _cancellationToken;
        private readonly Mock<HttpContextBase> _httpContextAccessorMock;
        private readonly HttpResponseClientDisconnectedTokenMediatorDecorator _sut;

        public HttpResponseClientDisconnectedTokenMediatorDecoratorTest()
        {
            _httpContextAccessorMock = new Mock<HttpContextBase>();
            _cancellationToken = new CancellationTokenSource().Token;
            var mediatorMock = new Mock<IMediator>();
            _sut = new HttpResponseClientDisconnectedTokenMediatorDecorator(
                mediatorMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Fact]
        public void GetCustomOrDefaultCancellationTokenShouldUseCancellationTokenWhenNonExistentHttpContext()
        {
            // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _httpContextAccessorMock
                .SetupGet(h => h.Response)
                .Returns(default(HttpResponseBase));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act
            var result = _sut.GetCustomOrDefaultCancellationToken(_cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(_cancellationToken);
            }
        }

        [Fact]
        public void GetCustomOrDefaultCancellationTokenShouldUseLinkedToken()
        {
            // Arrange
            var httpResponseMock = new Mock<HttpResponseBase>();
            var httpCancellationToken = default(CancellationToken);
            httpResponseMock
                .SetupGet(h => h.ClientDisconnectedToken)
                .Returns(httpCancellationToken);
            _httpContextAccessorMock
                .SetupGet(h => h.Response)
                .Returns(httpResponseMock.Object);

            // Act
            var result = _sut.GetCustomOrDefaultCancellationToken(_cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBe(_cancellationToken);
                result.Should().NotBe(httpCancellationToken);
            }
        }
    }
}
