using System;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test;

public sealed class HttpRequestAbortedMediatorDecoratorTest
    : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly HttpRequestAbortedMediatorDecorator _sut;

    public HttpRequestAbortedMediatorDecoratorTest()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        var mediatorMock = new Mock<IMediator>();
        _sut = new HttpRequestAbortedMediatorDecorator(
            mediatorMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCustomOrDefaultCancellationTokenShouldUseCancellationTokenWhenNonExistentHttpContext()
    {
        // Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _httpContextAccessorMock
            .SetupGet(h => h.HttpContext)
            .Returns(() => null!);
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
    public void GetCustomOrDefaultCancellationTokenShouldUseHttpContextToken()
    {
        // Arrange
        var httpCancellationToken = default(CancellationToken);
        _httpContextAccessorMock
            .SetupGet(h => h.HttpContext)
            .Returns(
                () =>
                    new DefaultHttpContext
                    {
                        RequestAborted = httpCancellationToken,
                    });

        // Act
        var result = _sut.GetCustomOrDefaultCancellationToken(_cancellationToken);

        // Assert
        using (new AssertionScope())
        {
            result.Should().Be(httpCancellationToken);
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
    }
}
