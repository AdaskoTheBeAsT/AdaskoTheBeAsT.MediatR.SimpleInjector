using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        private readonly TextWriter _writer;

        public PingHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return HandleInternal(request);
        }

        internal async Task<Pong> HandleInternal(Ping request)
        {
            await _writer.WriteLineAsync($"--- Handled Ping: {request.Message}");

            if (request.Throw)
            {
                throw new ApplicationException("Requested to throw");
            }

            return new Pong { Message = request.Message + " Pong" };
        }
    }
}
