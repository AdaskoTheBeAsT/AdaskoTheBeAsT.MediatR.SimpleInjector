using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore
{
    public class HttpRequestAbortedCancellationTokenMediatorDecorator
        : IMediator
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRequestAbortedCancellationTokenMediatorDecorator(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            var cancellationTokenToUse = GetRequestAbortedOrDefaultCancellationToken(cancellationToken);
            return _mediator.Send(request, cancellationTokenToUse);
        }

        public Task<object?> Send(
            object request,
            CancellationToken cancellationToken = default)
        {
            var cancellationTokenToUse = GetRequestAbortedOrDefaultCancellationToken(cancellationToken);
            return _mediator.Send(request, cancellationTokenToUse);
        }

        public Task Publish(
            object notification,
            CancellationToken cancellationToken = default)
        {
            var cancellationTokenToUse = GetRequestAbortedOrDefaultCancellationToken(cancellationToken);
            return _mediator.Publish(notification, cancellationTokenToUse);
        }

        public Task Publish<TNotification>(
            TNotification notification,
            CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var cancellationTokenToUse = GetRequestAbortedOrDefaultCancellationToken(cancellationToken);
            return _mediator.Publish(notification, cancellationTokenToUse);
        }

        private CancellationToken GetRequestAbortedOrDefaultCancellationToken(CancellationToken cancellationToken)
        {
            return _httpContextAccessor.HttpContext?.RequestAborted
                ?? cancellationToken;
        }
    }
}
