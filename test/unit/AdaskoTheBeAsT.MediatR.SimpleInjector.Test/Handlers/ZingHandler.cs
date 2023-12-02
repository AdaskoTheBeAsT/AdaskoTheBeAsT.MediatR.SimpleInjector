using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public class ZingHandler : IRequestHandler<Zing, Zong>
{
    private readonly Logger _output;

    public ZingHandler(Logger output)
    {
        _output = output;
    }

    public Task<Zong> Handle(Zing request, CancellationToken cancellationToken)
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

        _output.Messages.Add("Handler");
        return Task.FromResult(new Zong { Message = request.Message + " Zong" });
    }
}
