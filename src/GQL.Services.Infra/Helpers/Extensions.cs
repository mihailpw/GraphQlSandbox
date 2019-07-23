using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GQL.Services.Infra.Helpers
{
    internal static class CustomAttributeProviderExtensions
    {
        public static string GetNameOrDefault(this ICustomAttributeProvider attributeProvider, string defaultName)
        {
            return attributeProvider.FindInAttributes<INameProvider>()?.Name
                ?? defaultName;
        }

        public static Type GetReturnTypeOrDefault(this ICustomAttributeProvider attributeProvider, Type defaultReturnType)
        {
            return attributeProvider.FindInAttributes<IReturnTypeProvider>()?.ReturnType
                ?? defaultReturnType;
        }

        public static string GetDescription(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<IDescriptionProvider>()?.Description;
        }

        public static string GetDeprecationReason(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<IDeprecationReasonProvider>()?.DeprecationReason;
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

    internal static class Extensions
    {
        public static T GetValueOrDefault<T>(this IDictionary<string, T> dictionary, string key, T defaultValue = default)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}