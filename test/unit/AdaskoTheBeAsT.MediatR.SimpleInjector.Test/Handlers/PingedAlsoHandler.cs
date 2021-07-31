using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public class PingedAlsoHandler : INotificationHandler<Pinged>
    {
        public Task Handle(Pinged notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
