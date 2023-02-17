using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

public abstract class CancellationTokenMediatorDecoratorBase
    : IMediator
{
    private readonly IMediator _mediator;

    protected CancellationTokenMediatorDecoratorBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.CreateStream(request, cancellationTokenToUse);
    }

    public IAsyncEnumerable<object?> CreateStream(
        object request,
        CancellationToken cancellationToken = default)
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.CreateStream(request, cancellationTokenToUse);
    }

    public Task Publish(
        object notification,
        CancellationToken cancellationToken = default)
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.Publish(notification, cancellationTokenToUse);
    }

    public Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.Publish(notification, cancellationTokenToUse);
    }

    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.Send(request, cancellationTokenToUse);
    }

    public Task Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.Send(request, cancellationTokenToUse);
    }

    public Task<object?> Send(
        object request,
        CancellationToken cancellationToken = default)
    {
        var cancellationTokenToUse = GetCustomOrDefaultCancellationToken(cancellationToken);
        return _mediator.Send(request, cancellationTokenToUse);
    }

    public abstract CancellationToken GetCustomOrDefaultCancellationToken(CancellationToken cancellationToken);
}
