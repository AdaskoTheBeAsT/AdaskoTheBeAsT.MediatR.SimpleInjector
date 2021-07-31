using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        private readonly Logger _logger;

        public PingHandler(Logger logger)
        {
            _logger = logger;
        }

        public Task<Pong> Handle(Ping request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _logger.Messages.Add("Handler");

            request.ThrowAction?.Invoke(request);

            return Task.FromResult(new Pong { Message = request.Message + " Pong" });
        }
    }
}
