using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidPipelineBehaviorTypeException
        : Exception
    {
        public InvalidPipelineBehaviorTypeException()
        {
        }

        public InvalidPipelineBehaviorTypeException(string message)
            : base(message)
        {
        }

        public InvalidPipelineBehaviorTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidPipelineBehaviorTypeException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
