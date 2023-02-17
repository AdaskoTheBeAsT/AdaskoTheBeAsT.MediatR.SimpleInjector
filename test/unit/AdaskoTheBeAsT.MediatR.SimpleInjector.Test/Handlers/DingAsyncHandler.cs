using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public class DingAsyncHandler : IRequestHandler<Ding>
{
    public Task Handle(Ding request, CancellationToken cancellationToken) => Task.CompletedTask;
}
