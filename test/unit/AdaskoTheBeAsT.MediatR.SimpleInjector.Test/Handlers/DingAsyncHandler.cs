using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public class DingAsyncHandler : IRequestHandler<Ding>
    {
        public Task<Unit> Handle(Ding request, CancellationToken cancellationToken) => Unit.Task;
    }
}
