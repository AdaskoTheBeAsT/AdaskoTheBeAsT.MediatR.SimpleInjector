using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidRequestPreProcessorTypeException
    : Exception
{
    public InvalidRequestPreProcessorTypeException()
    {
    }

    public InvalidRequestPreProcessorTypeException(string message)
        : base(message)
    {
    }

    public InvalidRequestPreProcessorTypeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected InvalidRequestPreProcessorTypeException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
