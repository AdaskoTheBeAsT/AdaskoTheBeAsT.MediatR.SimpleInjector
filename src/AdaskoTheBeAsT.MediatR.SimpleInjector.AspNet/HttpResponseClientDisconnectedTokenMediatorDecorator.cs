using System.Threading;
using System.Web;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    public class HttpResponseClientDisconnectedTokenMediatorDecorator
        : CancellationTokenMediatorDecoratorBase
    {
        private readonly HttpContextBase _httpContextAccessor;

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
