using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace TestApp
{
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
}
