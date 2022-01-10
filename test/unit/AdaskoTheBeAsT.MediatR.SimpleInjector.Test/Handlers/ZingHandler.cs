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
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        _output.Messages.Add("Handler");
        return Task.FromResult(new Zong { Message = request.Message + " Zong" });
    }
}
