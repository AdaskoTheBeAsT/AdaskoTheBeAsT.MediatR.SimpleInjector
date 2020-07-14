using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class HttpContextCreatorReturnsNullException
        : Exception
    {
        public HttpContextCreatorReturnsNullException()
        {
        }

        public HttpContextCreatorReturnsNullException(string message)
            : base(message)
        {
        }

        public HttpContextCreatorReturnsNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HttpContextCreatorReturnsNullException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
