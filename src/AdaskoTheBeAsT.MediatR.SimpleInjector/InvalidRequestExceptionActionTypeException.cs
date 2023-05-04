using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

[ExcludeFromCodeCoverage]
[Serializable]
public class InvalidRequestExceptionActionTypeException
    : Exception
{
    public InvalidRequestExceptionActionTypeException()
    {
    }

    public InvalidRequestExceptionActionTypeException(string message)
        : base(message)
    {
    }

    public InvalidRequestExceptionActionTypeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected InvalidRequestExceptionActionTypeException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
