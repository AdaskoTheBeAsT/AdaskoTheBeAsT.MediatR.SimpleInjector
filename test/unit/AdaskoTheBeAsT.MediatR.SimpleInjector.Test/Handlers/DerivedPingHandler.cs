using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public class DerivedPingHandler : IRequestHandler<DerivedPing, Pong>
{
    private readonly Logger _logger;

    public DerivedPingHandler(Logger logger)
    {
        _logger = logger;
    }

    public Task<Pong> Handle(DerivedPing request, CancellationToken cancellationToken)
    {
#if NET462_OR_GREATER
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
#endif

#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(request);
#endif

        _logger.Messages.Add("Handler");
        return Task.FromResult(new Pong { Message = $"Derived{request.Message} Pong" });
    }
}
