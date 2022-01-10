using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public sealed class Ding : IRequest
{
    public string? Message { get; set; }
}
