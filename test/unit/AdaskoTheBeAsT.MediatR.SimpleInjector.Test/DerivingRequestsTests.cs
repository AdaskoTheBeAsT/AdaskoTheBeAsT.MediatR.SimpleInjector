using System;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class DerivingRequestsTests
    : IDisposable
{
    private readonly Container _container;
    private readonly IMediator _mediator;

    public DerivingRequestsTests()
    {
        _container = new Container();
        _container.RegisterSingleton<Logger>();
        _container.AddMediatR(typeof(Ping));
        _mediator = _container.GetInstance<IMediator>();
    }

    [Fact]
    public async Task ShouldReturnPingPongAsync()
    {
#pragma warning disable CC0021 // Use nameof
        var pong = await _mediator.Send(new Ping { Message = "Ping" });
#pragma warning restore CC0021 // Use nameof
        pong.Message.Should().Be("Ping Pong");
    }

    [Fact]
    public async Task ShouldReturnDerivedPingPongAsync()
    {
#pragma warning disable CC0021 // Use nameof
        var pong = await _mediator.Send(new DerivedPing { Message = "Ping" });
#pragma warning restore CC0021 // Use nameof
        pong.Message.Should().Be("DerivedPing Pong");
    }

    public void Dispose()
    {
        _container.Dispose();
    }
}
