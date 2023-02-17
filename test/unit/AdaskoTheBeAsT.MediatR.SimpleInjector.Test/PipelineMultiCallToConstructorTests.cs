using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test;
#pragma warning disable CA1812
public class PipelineMultiCallToConstructorTests
{
    [Fact]
    public async Task ShouldNotCallConstructorMultipleTimesWhenUsingAPipelineAsync()
    {
        var output = new Logger();
#if NET6_0_OR_GREATER
        await using var container = new Container();
#else
        using var container = new Container();
#endif
        container.RegisterInstance(output);
        container.AddMediatR(
            config =>
            {
                config.WithHandlerAssemblyMarkerTypes(typeof(Ping));
                config.UsingBuiltinPipelineProcessorBehaviors(
                    requestPreProcessorBehaviorEnabled: true,
                    requestPostProcessorBehaviorEnabled: true,
                    requestExceptionProcessorBehaviorEnabled: true,
                    requestExceptionActionProcessorBehaviorEnabled: true);
                config.UsingPipelineProcessorBehaviors(typeof(ConstructorTestBehavior<,>));
            });

        var mediator = container.GetInstance<IMediator>();

        var response = await mediator.Send(new ConstructorTestRequest { Message = "ConstructorPing" });

        response.Message.Should().Be("ConstructorPing ConstructorPong");

        output.Messages.Should().BeEquivalentTo(
            "ConstructorTestBehavior before",
            "First pre processor",
            "Next pre processor",
            "Handler",
            "First post processor",
            "Next post processor",
            "ConstructorTestBehavior after");
    }

    [Fact]
    public async Task ShouldNotCallConstructorMultipleTimesWhenUsingAStreamPipelineAsync()
    {
        var output = new Logger();
#if NET6_0_OR_GREATER
        await using var container = new Container();
#else
        using var container = new Container();
#endif
        container.RegisterInstance(output);
        container.AddMediatR(
            config =>
            {
                config.WithHandlerAssemblyMarkerTypes(typeof(Ping));
                config.UsingBuiltinPipelineProcessorBehaviors(
                    requestPreProcessorBehaviorEnabled: false,
                    requestPostProcessorBehaviorEnabled: false,
                    requestExceptionProcessorBehaviorEnabled: false,
                    requestExceptionActionProcessorBehaviorEnabled: false);
                config.UsingStreamPipelineBehaviors(typeof(StreamConstructorTestBehavior<,>));
            });

        var mediator = container.GetInstance<IMediator>();

        await foreach (var item in mediator.CreateStream(
                           new StreamConstructorTestRequest { Message = "ConstructorPing" }))
        {
            item.Message.Should().Be("ConstructorPing ConstructorPong");
        }

        output.Messages.Should().BeEquivalentTo(
            "StreamConstructorTestBehavior before", "Handler");
    }

    public sealed class StreamConstructorTestBehavior<TRequest, TResponse>
        : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
    {
        private readonly Logger _output;

        public StreamConstructorTestBehavior(Logger output) => _output = output;

        public IAsyncEnumerable<TResponse> Handle(
            TRequest request,
            StreamHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _output.Messages.Add("StreamConstructorTestBehavior before");
#pragma warning disable CC0031 // Check for null before calling a delegate
            return next();
#pragma warning restore CC0031 // Check for null before calling a delegate
        }
    }

    public sealed class StreamConstructorTestRequest : IStreamRequest<StreamConstructorTestResponse>
    {
        public string? Message { get; set; }
    }

    public sealed class StreamConstructorTestResponse
    {
        public string? Message { get; set; }
    }

    public sealed class StreamConstructorTestHandler
        : IStreamRequestHandler<StreamConstructorTestRequest, StreamConstructorTestResponse>
    {
        private readonly Logger _logger;

        public StreamConstructorTestHandler(Logger logger)
        {
            _logger = logger;
        }

        public IAsyncEnumerable<StreamConstructorTestResponse> Handle(
            StreamConstructorTestRequest request,
            CancellationToken cancellationToken)
        {
            _logger.Messages.Add("Handler");
            return new[] { new StreamConstructorTestResponse { Message = request.Message + " ConstructorPong" } }.ToAsyncEnumerable();
        }
    }

    internal sealed class ConstructorTestBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Logger _output;

        public ConstructorTestBehavior(Logger output) => _output = output;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _output.Messages.Add("ConstructorTestBehavior before");
#pragma warning disable CC0031 // Check for null before calling a delegate
            var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
            _output.Messages.Add("ConstructorTestBehavior after");

            return response;
        }
    }

    internal sealed class ConstructorTestRequest : IRequest<ConstructorTestResponse>
    {
        public string? Message { get; set; }
    }

    internal sealed class ConstructorTestResponse
    {
        public string? Message { get; set; }
    }

    internal sealed class ConstructorTestHandler : IRequestHandler<ConstructorTestRequest, ConstructorTestResponse>
    {
        private readonly Logger _logger;

        public ConstructorTestHandler(Logger logger)
        {
            _logger = logger;
        }

        public Task<ConstructorTestResponse> Handle(ConstructorTestRequest request, CancellationToken cancellationToken)
        {
            _logger.Messages.Add("Handler");
            return Task.FromResult(new ConstructorTestResponse { Message = request.Message + " ConstructorPong" });
        }
    }
}
#pragma warning restore CA1812
