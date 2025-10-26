using System;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using AwesomeAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Xunit;
using Xunit.Abstractions;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;

public sealed class DerivingRequestsTests
    : IDisposable
{
    private readonly Container _container;
    private readonly IMediator _mediator;

    public DerivingRequestsTests(ITestOutputHelper output)
    {
        _container = new Container();
        _container.RegisterInstance(output);

        // ILoggerFactory that writes to test output
        _container.RegisterSingleton<ILoggerFactory>(() =>
            LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
#pragma warning disable IDISP004
                builder.AddProvider(new XunitTestOutputLoggerProvider(output));
#pragma warning restore IDISP004
                builder.SetMinimumLevel(LogLevel.Trace);
            }));

        // Wire up ILogger<T> using the factory
        _container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Singleton);

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
