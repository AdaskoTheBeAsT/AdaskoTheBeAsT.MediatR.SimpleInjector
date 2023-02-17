using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

internal sealed class InternalPingHandler : IRequestHandler<InternalPing>
{
    public Task Handle(InternalPing request, CancellationToken cancellationToken) => Task.CompletedTask;
}
