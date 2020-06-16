using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    public class HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator
        : IMediator
    {
        private readonly IMediator _mediator;
        private readonly HttpContextBase _httpContextAccessor;

        public HttpResponseClientDisconnectedTokenCancellationTokenMediatorDecorator(
            IMediator mediator,
            HttpContextBase httpContextAccessor)
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

        public Task<object> Send(
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
            var response = _httpContextAccessor.Response;
            if (response == null)
            {
                return cancellationToken;
            }

            var disconnectedToken = response.ClientDisconnectedToken;
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disconnectedToken);

            return linkedTokenSource.Token;
        }
    }
}
