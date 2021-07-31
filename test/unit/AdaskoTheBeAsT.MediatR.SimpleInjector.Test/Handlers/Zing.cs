using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public sealed class Zing : IRequest<Zong>
    {
        public string? Message { get; set; }
    }
}
