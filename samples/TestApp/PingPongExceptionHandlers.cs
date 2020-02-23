using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace TestApp
{
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name
    public class PingPongExceptionHandlerForType : IRequestExceptionHandler<Ping, Pong, ApplicationException>
    {
        public Task Handle(Ping request, ApplicationException exception, RequestExceptionHandlerState<Pong> state, CancellationToken cancellationToken)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            state.SetHandled(new Pong { Message = exception.Message + " Handled by Type" });

            return Task.CompletedTask;
        }
    }

    public class PingPongExceptionActionForType1 : IRequestExceptionAction<Ping, ApplicationException>
    {
        private readonly TextWriter _output;

        public PingPongExceptionActionForType1(TextWriter output) => _output = output;

        public Task Execute(Ping request, ApplicationException exception, CancellationToken cancellationToken)
            => _output.WriteLineAsync("Logging exception 1");
    }

    public class PingPongExceptionActionForType2 : IRequestExceptionAction<Ping, ApplicationException>
    {
        private readonly TextWriter _output;

        public PingPongExceptionActionForType2(TextWriter output) => _output = output;

        public Task Execute(Ping request, ApplicationException exception, CancellationToken cancellationToken)
            => _output.WriteLineAsync("Logging exception 2");
    }
#pragma warning restore SA1649 // File name should match first type name
#pragma warning restore SA1402 // File may only contain a single type
}
