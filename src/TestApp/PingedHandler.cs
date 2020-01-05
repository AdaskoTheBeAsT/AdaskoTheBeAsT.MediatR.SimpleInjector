using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace TestApp
{
#pragma warning disable SA1402 // File may only contain a single type
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

    public class ConstrainedPingedHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : Pinged
    {
        private readonly TextWriter _writer;

        public ConstrainedPingedHandler(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("Got pinged constrained async.");
        }
    }

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
#pragma warning restore SA1402 // File may only contain a single type
}
