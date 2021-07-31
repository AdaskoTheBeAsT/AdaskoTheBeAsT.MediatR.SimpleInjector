using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace TestApp
{
    public class PingPongExceptionActionForType1 : IRequestExceptionAction<Ping, ApplicationException>
    {
        private readonly TextWriter _output;

#pragma warning disable CC0057 // Unused parameters
        public PingPongExceptionActionForType1(TextWriter output) => _output = output;
#pragma warning restore CC0057 // Unused parameters

        public Task Execute(Ping request, ApplicationException exception, CancellationToken cancellationToken)
            => _output.WriteLineAsync("Logging exception 1");
    }
}
