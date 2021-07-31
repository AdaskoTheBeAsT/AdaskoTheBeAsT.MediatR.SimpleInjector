using System;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public class Ping : IRequest<Pong>
    {
        public string? Message { get; set; }

        public Action<Ping>? ThrowAction { get; set; }
    }
}
