using MediatR;

namespace TestApp
{
    public class Jing : IRequest
    {
        public string? Message { get; set; }
    }
}
