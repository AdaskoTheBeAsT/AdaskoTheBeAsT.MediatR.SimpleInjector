using System.Threading;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
#pragma warning disable CA1812
    public class PipelineMultiCallToConstructorTests
    {
        [Fact]
        public async Task ShouldNotCallConstructorMultipleTimesWhenUsingAPipelineAsync()
        {
            var output = new Logger();
#if NET461 || NETCOREAPP2_1
            using var container = new Container();
#else
            await using var container = new Container();
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

        internal sealed class ConstructorTestBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public ConstructorTestBehavior(Logger output) => _output = output;

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _output.Messages.Add("ConstructorTestBehavior before");
                var response = await next!().ConfigureAwait(false);
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
}
