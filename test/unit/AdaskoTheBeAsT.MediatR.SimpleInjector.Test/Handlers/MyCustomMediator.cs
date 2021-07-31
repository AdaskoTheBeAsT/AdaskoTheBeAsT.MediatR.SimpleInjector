using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    internal sealed class MyCustomMediator : IMediator
    {
        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            throw new NotSupportedException();
        }
    }
}
