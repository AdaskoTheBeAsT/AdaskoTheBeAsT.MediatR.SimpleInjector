using System.Threading;
using System.Threading.Tasks;
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
        public async Task ShouldNotCallConstructorMultipleTimesWhenUsingAPipeline()
        {
            ConstructorTestHandler.ResetCallCount();
            ConstructorTestHandler.ConstructorCallCount.Should().Be(0);

            var output = new Logger();
            using var container = new Container();

            container.RegisterInstance(output);
            container.AddMediatR(
                config =>
                {
                    config.WithHandlerAssemblyMarkerTypes(typeof(Ping));
                    config.UsingBuiltinPipelineProcessorBehaviors(
                            true,
                            true,
                            true,
                            true);
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
            ConstructorTestHandler.ConstructorCallCount.Should().Be(1);
        }

        internal class ConstructorTestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        {
            private readonly Logger _output;

            public ConstructorTestBehavior(Logger output) => _output = output;

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _output.Messages.Add("ConstructorTestBehavior before");
                var response = await next();
                _output.Messages.Add("ConstructorTestBehavior after");

                return response;
            }
        }

        internal class ConstructorTestRequest : IRequest<ConstructorTestResponse>
        {
            public string? Message { get; set; }
        }

        internal class ConstructorTestResponse
        {
            public string? Message { get; set; }
        }

        internal class ConstructorTestHandler : IRequestHandler<ConstructorTestRequest, ConstructorTestResponse>
        {
            private static volatile object _lockObject = new object();
            private static int _constructorCallCount;
            private readonly Logger _logger;

            public ConstructorTestHandler(Logger logger)
            {
                _logger = logger;
                lock (_lockObject)
                {
                    _constructorCallCount++;
                }
            }

            public static int ConstructorCallCount => _constructorCallCount;

            public static void ResetCallCount()
            {
                lock (_lockObject)
                {
                    _constructorCallCount = 0;
                }
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
