using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore;

public class HttpRequestAbortedMediatorDecorator
    : CancellationTokenMediatorDecoratorBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpRequestAbortedMediatorDecorator(
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor)
        : base(mediator)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override CancellationToken GetCustomOrDefaultCancellationToken(CancellationToken cancellationToken)
    {
        return _httpContextAccessor.HttpContext?.RequestAborted
               ?? cancellationToken;
    }
}
