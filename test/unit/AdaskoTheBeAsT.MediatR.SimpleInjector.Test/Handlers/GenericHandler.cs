using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public class GenericHandler : INotificationHandler<INotification>
{
    public Task Handle(INotification notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
