using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public class PingedAlsoHandler : INotificationHandler<Pinged>
    {
        private readonly TextWriter _writer;

        public PingedAlsoHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(Pinged notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("Got pinged also async.");
        }
    }
}
