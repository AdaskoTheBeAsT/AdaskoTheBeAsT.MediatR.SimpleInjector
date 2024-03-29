using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNetCore.Test;

public class FakeMediator
    : IMediator
{
    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult((TResponse)new object());
    }

    public Task Send<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        return Task.CompletedTask;
    }

    public Task<object?> Send(
        object request,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        return Task.FromResult<object?>(new object());
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        return Array.Empty<TResponse>().ToAsyncEnumerable();
    }

    public IAsyncEnumerable<object?> CreateStream(
        object request,
        CancellationToken cancellationToken = default)
    {
        return Array.Empty<object?>().ToAsyncEnumerable();
    }

    public Task Publish(
        object notification,
        CancellationToken cancellationToken = default)
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        return Unit.Task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
    }

    public Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        return Unit.Task;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
    }
}
