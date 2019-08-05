using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.Services.Infra.Attributes;
using GQL.Services.Infra.Providers;

namespace GQL.Services.Infra.Common.Helpers
{
    internal static class EnumerableExtensions
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable, int length)
        {
            var array = new T[length];
            var i = 0;
            foreach (var value in enumerable)
            {
                array[i] = value;
                i++;
            }

            return array;
        }
    }

    internal static class CustomAttributeProviderExtensions
    {
        public static string GetNameOrDefault(this ICustomAttributeProvider attributeProvider, string defaultName)
        {
            return attributeProvider.FindInAttributes<INameProvider>()?.Name ?? defaultName;
        }

        public static bool IsGraphQlMember(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<GraphQlAttribute>() != null;
        }

        public static T FindInAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit = false)
        {
            return attributeProvider.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault();
        }
    }

    internal static class TypeExtensions
    {
        public static bool IsGenericTypeDefinition(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }
    }
}