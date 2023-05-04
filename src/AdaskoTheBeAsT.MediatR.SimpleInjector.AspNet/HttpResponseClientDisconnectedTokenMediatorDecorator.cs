using System;
using System.Threading;
using System.Web;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    public sealed class HttpResponseClientDisconnectedTokenMediatorDecorator
        : CancellationTokenMediatorDecoratorBase,
            IDisposable
    {
        private readonly HttpContextBase _httpContextAccessor;
        private CancellationTokenSource? _cancellationTokenSource;

        public HttpResponseClientDisconnectedTokenMediatorDecorator(
            IMediator mediator,
            HttpContextBase httpContextAccessor)
            : base(mediator)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override CancellationToken GetCustomOrDefaultCancellationToken(CancellationToken cancellationToken)
        {
            var response = _httpContextAccessor.Response;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (response == null)
            {
                return cancellationToken;
            }

            var disconnectedToken = response.ClientDisconnectedToken;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disconnectedToken);

            return _cancellationTokenSource.Token;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
