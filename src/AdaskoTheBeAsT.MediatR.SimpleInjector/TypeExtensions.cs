using System;

namespace AdaskoTheBeAsT.MediatR.SimpleInjector;

internal static class TypeExtensions
{
    internal static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        foreach (var type in givenType.GetInterfaces())
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        if (baseType == null)
        {
            return false;
        }

        return IsAssignableToGenericType(baseType, genericType);
    }
}
