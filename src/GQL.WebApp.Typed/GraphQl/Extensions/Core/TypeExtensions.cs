using System;

namespace GQL.WebApp.Typed.GraphQl.Extensions.Core
{
    internal static class TypeExtensions
    {
        public static bool IsGenericTypeDefinition(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }
    }
}