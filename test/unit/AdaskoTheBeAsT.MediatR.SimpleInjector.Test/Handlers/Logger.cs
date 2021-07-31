using System.Collections.Generic;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.Test.Handlers
{
    public class Logger
    {
        public IList<string> Messages { get; } = new List<string>();
    }
}
