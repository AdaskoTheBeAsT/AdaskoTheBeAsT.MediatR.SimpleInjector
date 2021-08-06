using System;
using System.Threading;
using System.Threading.Tasks;
using AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;
using FluentAssertions;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using Xunit;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test
{
#pragma warning disable CA1812
    public class PipelineTests
    {
        [Fact]
        public async Task ShouldWrapWithBehaviorAsync()
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
                    config.UsingPipelineProcessorBehaviors(typeof(OuterBehavior), typeof(InnerBehavior));
                });

            var mediator = container.GetInstance<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo(
                "Outer before",
                "Inner before",
                "First concrete pre processor",
                "Next concrete pre processor",
                "First pre processor",
                "Next pre processor",
                "Handler",
                "First concrete post processor",
                "Next concrete post processor",
                "First post processor",
                "Next post processor",
                "Inner after",
                "Outer after");
        }

        [Fact]
        public async Task ShouldWrapGenericsWithBehaviorAsync()
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
                    config.UsingPipelineProcessorBehaviors(
                        typeof(OuterBehavior<,>),
                        typeof(InnerBehavior<,>));
                });

            var mediator = container.GetInstance<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo(
                "Outer generic before",
                "Inner generic before",
                "First concrete pre processor",
                "Next concrete pre processor",
                "First pre processor",
                "Next pre processor",
                "Handler",
                "First concrete post processor",
                "Next concrete post processor",
                "First post processor",
                "Next post processor",
                "Inner generic after",
                "Outer generic after");
        }

        [Fact]
        public async Task ShouldPickUpPreAndPostProcessorsAsync()
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
                });

            var mediator = container.GetInstance<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo(
                "First concrete pre processor",
                "Next concrete pre processor",
                "First pre processor",
                "Next pre processor",
                "Handler",
                "First concrete post processor",
                "Next concrete post processor",
                "First post processor",
                "Next post processor");
        }

        [Fact]
        public void ShouldPickUpBaseExceptionBehaviors()
        {
            var output = new Logger();
            using var container = new Container();
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
                });

            var mediator = container.GetInstance<IMediator>();

#pragma warning disable S3626 // Jump statements should not be redundant
            Func<Task> action = async () => await mediator.Send(
                    new Ping
                    {
                        Message = "Ping",
                        ThrowAction = msg => throw new Exception(msg.Message + " Thrown"),
                    })
                .ConfigureAwait(false);
#pragma warning restore S3626 // Jump statements should not be redundant

            action.Should().Throw<Exception>();

            output.Messages.Should().Contain("Ping Thrown Logged by Generic Type");
            output.Messages.Should().Contain("Logging generic exception");
        }

        [Fact]
        public void ShouldPickUpExceptionActions()
        {
            var output = new Logger();
            using var container = new Container();
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
                });

            var mediator = container.GetInstance<IMediator>();

#pragma warning disable S3626 // Jump statements should not be redundant
            Func<Task> action = async () => await mediator.Send(
                    new Ping
                    {
                        Message = "Ping",
                        ThrowAction = msg => throw new SystemException(msg.Message + " Thrown"),
                    })
                .ConfigureAwait(false);
#pragma warning restore S3626 // Jump statements should not be redundant

            action.Should().Throw<SystemException>();

            output.Messages.Should().Contain("Logging exception 1");
            output.Messages.Should().Contain("Logging exception 2");
        }

        [Fact]
        public async Task ShouldHandleConstrainedGenericsAsync()
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
                    config.UsingPipelineProcessorBehaviors(
                        typeof(OuterBehavior<,>),
                        typeof(InnerBehavior<,>),
                        typeof(ConstrainedBehavior<,>));
                });

            var mediator = container.GetInstance<IMediator>();

            var response = await mediator.Send(new Ping { Message = "Ping" });

            response.Message.Should().Be("Ping Pong");

            output.Messages.Should().BeEquivalentTo(
                "Outer generic before",
                "Inner generic before",
                "Constrained before",
                "Handler",
                "Constrained after",
                "Inner generic after",
                "Outer generic after");

            output.Messages.Clear();

            var zingResponse = await mediator.Send(new Zing { Message = "Zing" });

            zingResponse.Message.Should().Be("Zing Zong");

            output.Messages.Should().BeEquivalentTo(
                "Outer generic before",
                "Inner generic before",
                "Handler",
                "Inner generic after",
                "Outer generic after");
        }

        internal class OuterBehavior : IPipelineBehavior<Ping, Pong>
        {
            private readonly Logger _output;

            public OuterBehavior(Logger output)
            {
                _output = output;
            }

            public async Task<Pong> Handle(Ping request, CancellationToken cancellationToken, RequestHandlerDelegate<Pong> next)
            {
                _output.Messages.Add("Outer before");
#pragma warning disable CC0031 // Check for null before calling a delegate
                var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
                _output.Messages.Add("Outer after");

                return response;
            }
        }

        internal class InnerBehavior : IPipelineBehavior<Ping, Pong>
        {
            private readonly Logger _output;

            public InnerBehavior(Logger output)
            {
                _output = output;
            }

            public async Task<Pong> Handle(Ping request, CancellationToken cancellationToken, RequestHandlerDelegate<Pong> next)
            {
                _output.Messages.Add("Inner before");
#pragma warning disable CC0031 // Check for null before calling a delegate
                var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
                _output.Messages.Add("Inner after");

                return response;
            }
        }

        internal class InnerBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public InnerBehavior(Logger output)
            {
                _output = output;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _output.Messages.Add("Inner generic before");
#pragma warning disable CC0031 // Check for null before calling a delegate
                var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
                _output.Messages.Add("Inner generic after");

                return response;
            }
        }

        internal class OuterBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public OuterBehavior(Logger output)
            {
                _output = output;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _output.Messages.Add("Outer generic before");
#pragma warning disable CC0031 // Check for null before calling a delegate
                var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
                _output.Messages.Add("Outer generic after");

                return response;
            }
        }

        internal class ConstrainedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : Ping
            where TResponse : Pong
        {
            private readonly Logger _output;

            public ConstrainedBehavior(Logger output)
            {
                _output = output;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
            {
                _output.Messages.Add("Constrained before");
#pragma warning disable CC0031 // Check for null before calling a delegate
                var response = await next().ConfigureAwait(false);
#pragma warning restore CC0031 // Check for null before calling a delegate
                _output.Messages.Add("Constrained after");

                return response;
            }
        }

        internal class FirstPreProcessor<TRequest>
            : IRequestPreProcessor<TRequest>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public FirstPreProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(TRequest request, CancellationToken cancellationToken)
            {
                _output.Messages.Add("First pre processor");
                return Task.CompletedTask;
            }
        }

        internal class FirstConcretePreProcessor : IRequestPreProcessor<Ping>
        {
            private readonly Logger _output;

            public FirstConcretePreProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(Ping request, CancellationToken cancellationToken)
            {
                _output.Messages.Add("First concrete pre processor");
                return Task.CompletedTask;
            }
        }

        internal class NextPreProcessor<TRequest>
            : IRequestPreProcessor<TRequest>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public NextPreProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(TRequest request, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Next pre processor");
                return Task.CompletedTask;
            }
        }

        internal class NextConcretePreProcessor : IRequestPreProcessor<Ping>
        {
            private readonly Logger _output;

            public NextConcretePreProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(Ping request, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Next concrete pre processor");
                return Task.CompletedTask;
            }
        }

        internal class FirstPostProcessor<TRequest, TResponse>
            : IRequestPostProcessor<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public FirstPostProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
            {
                _output.Messages.Add("First post processor");
                return Task.CompletedTask;
            }
        }

        internal class FirstConcretePostProcessor : IRequestPostProcessor<Ping, Pong>
        {
            private readonly Logger _output;

            public FirstConcretePostProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(Ping request, Pong response, CancellationToken cancellationToken)
            {
                _output.Messages.Add("First concrete post processor");
                return Task.CompletedTask;
            }
        }

        internal class NextPostProcessor<TRequest, TResponse>
            : IRequestPostProcessor<TRequest, TResponse>
            where TRequest : notnull
        {
            private readonly Logger _output;

            public NextPostProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Next post processor");
                return Task.CompletedTask;
            }
        }

        internal class NextConcretePostProcessor : IRequestPostProcessor<Ping, Pong>
        {
            private readonly Logger _output;

            public NextConcretePostProcessor(Logger output)
            {
                _output = output;
            }

            public Task Process(Ping request, Pong response, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Next concrete post processor");
                return Task.CompletedTask;
            }
        }

        internal class PingPongGenericExceptionAction : IRequestExceptionAction<Ping>
        {
            private readonly Logger _output;

            public PingPongGenericExceptionAction(Logger output) => _output = output;

            public Task Execute(Ping request, Exception exception, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Logging generic exception");

                return Task.CompletedTask;
            }
        }

        internal class PingPongApplicationExceptionAction : IRequestExceptionAction<Ping, ApplicationException>
        {
            private readonly Logger _output;

            public PingPongApplicationExceptionAction(Logger output) => _output = output;

            public Task Execute(Ping request, ApplicationException exception, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Logging ApplicationException exception");

                return Task.CompletedTask;
            }
        }

        internal class PingPongExceptionActionForType1 : IRequestExceptionAction<Ping, SystemException>
        {
            private readonly Logger _output;

            public PingPongExceptionActionForType1(Logger output) => _output = output;

            public Task Execute(Ping request, SystemException exception, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Logging exception 1");

                return Task.CompletedTask;
            }
        }

        internal class PingPongExceptionActionForType2 : IRequestExceptionAction<Ping, SystemException>
        {
            private readonly Logger _output;

            public PingPongExceptionActionForType2(Logger output) => _output = output;

            public Task Execute(Ping request, SystemException exception, CancellationToken cancellationToken)
            {
                _output.Messages.Add("Logging exception 2");

                return Task.CompletedTask;
            }
        }

        internal class PingPongExceptionHandlerForType : IRequestExceptionHandler<Ping, Pong, ApplicationException>
        {
            public Task Handle(Ping request, ApplicationException exception, RequestExceptionHandlerState<Pong> state, CancellationToken cancellationToken)
            {
                state.SetHandled(new Pong { Message = exception.Message + " Handled by Specific Type" });

                return Task.CompletedTask;
            }
        }

        internal class PingPongGenericExceptionHandler : IRequestExceptionHandler<Ping, Pong>
        {
            private readonly Logger _output;

            public PingPongGenericExceptionHandler(Logger output) => _output = output;

            public Task Handle(Ping request, Exception exception, RequestExceptionHandlerState<Pong> state, CancellationToken cancellationToken)
            {
                _output.Messages.Add(exception.Message + " Logged by Generic Type");

                return Task.CompletedTask;
            }
        }
    }
#pragma warning restore CA1812
}
