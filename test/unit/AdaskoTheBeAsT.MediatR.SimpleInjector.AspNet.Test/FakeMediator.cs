using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet.Test
{
    public class FakeMediator
        : IMediator
    {
        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult((TResponse)new object());
        }

        public Task<object?> Send(
            object request,
            CancellationToken cancellationToken = default)
        {
            // ReSharper disable once RedundantTypeArgumentsOfMethod
            return Task.FromResult<object?>(new object());
        }

        public Task Publish(
            object notification,
            CancellationToken cancellationToken = default)
        {
            return Unit.Task;
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Unit.Task;
        }
    }
}
