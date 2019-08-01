using System;

namespace GQL.Annotations.Utilities
{
    internal static class ActivatorHelper
    {
        public static T CreateInstance<T>(Type genericType, params Type[] typeArguments)
        {
            return (T) Activator.CreateInstance(genericType.MakeGenericType(typeArguments));
        }

        public static object CreateDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}