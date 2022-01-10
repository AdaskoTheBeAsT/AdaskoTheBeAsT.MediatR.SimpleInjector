using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers;

public class DuplicateHandler1 : IRequestHandler<DuplicateTest, string>
{
    public Task<string> Handle(DuplicateTest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(nameof(DuplicateHandler1));
    }
}
