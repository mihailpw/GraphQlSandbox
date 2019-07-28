using System;
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

        public static bool IsRequired(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<IRequiredProvider>()?.IsRequired == true;
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