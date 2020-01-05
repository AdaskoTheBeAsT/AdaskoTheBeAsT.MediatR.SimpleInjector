using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public class JingHandler : AsyncRequestHandler<Jing>
    {
        private readonly TextWriter _writer;

        public JingHandler(TextWriter writer)
        {
            _writer = writer;
        }

        protected override Task Handle(Jing request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _writer.WriteLineAsync($"--- Handled Jing: {request.Message}, no Jong");
        }
    }
}
