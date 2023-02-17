using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector.AspNet
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidAspNetLifetimeException
        : Exception
    {
        public InvalidAspNetLifetimeException()
        {
        }

        public InvalidAspNetLifetimeException(string message)
            : base(message)
        {
        }

        public InvalidAspNetLifetimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidAspNetLifetimeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
