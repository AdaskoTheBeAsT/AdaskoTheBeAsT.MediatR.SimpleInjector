using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test
{
    public class FakeMediator<TResp>
        : IMediator
    {
        public IRequest<TResp>? SendRequestTyped { get; private set; }

        public object? SendRequestObject { get; private set; }

        public INotification? PublishNotificationTyped { get; private set; }

        public object? PublishNotificationObject { get; private set; }

        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            SendRequestTyped = (IRequest<TResp>)request;
            return Task.FromResult((TResponse)new object());
        }

        public Task<object> Send(
            object request,
            CancellationToken cancellationToken = default)
        {
            SendRequestObject = request;
            return Task.FromResult(new object());
        }

        public Task Publish(
            object notification,
            CancellationToken cancellationToken = default)
        {
            PublishNotificationObject = notification;
            return Unit.Task;
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            PublishNotificationTyped = notification;
            return Unit.Task;
        }
    }
}
