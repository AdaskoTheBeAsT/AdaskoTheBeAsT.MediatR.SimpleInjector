using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidRequestExceptionHandlerTypeException
    : Exception
{
    public InvalidRequestExceptionHandlerTypeException()
    {
    }

    public InvalidRequestExceptionHandlerTypeException(string message)
        : base(message)
    {
    }

    public InvalidRequestExceptionHandlerTypeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected InvalidRequestExceptionHandlerTypeException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
