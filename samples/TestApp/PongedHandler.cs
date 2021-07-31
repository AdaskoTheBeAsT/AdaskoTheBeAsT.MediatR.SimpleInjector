using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
    public class PongedHandler : INotificationHandler<Ponged>
    {
        private readonly TextWriter _writer;

        public PongedHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(Ponged notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("Got ponged async.");
        }
    }
}
