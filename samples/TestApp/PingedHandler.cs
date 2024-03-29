using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public class PingedHandler : INotificationHandler<Pinged>
    {
        private readonly TextWriter _writer;

        public PingedHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(Pinged notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("Got pinged async.");
        }
    }
}
