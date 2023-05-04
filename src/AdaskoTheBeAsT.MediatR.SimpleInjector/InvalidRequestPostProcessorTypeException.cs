using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidRequestPostProcessorTypeException
    : Exception
{
    public InvalidRequestPostProcessorTypeException()
    {
    }

    public InvalidRequestPostProcessorTypeException(string message)
        : base(message)
    {
    }

    public InvalidRequestPostProcessorTypeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected InvalidRequestPostProcessorTypeException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
