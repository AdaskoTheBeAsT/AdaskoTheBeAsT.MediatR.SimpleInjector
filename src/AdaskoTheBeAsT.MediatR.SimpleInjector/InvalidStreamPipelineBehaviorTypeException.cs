using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidStreamPipelineBehaviorTypeException
    : Exception
{
    public InvalidStreamPipelineBehaviorTypeException()
    {
    }

    public InvalidStreamPipelineBehaviorTypeException(string message)
        : base(message)
    {
    }

    public InvalidStreamPipelineBehaviorTypeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected InvalidStreamPipelineBehaviorTypeException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
