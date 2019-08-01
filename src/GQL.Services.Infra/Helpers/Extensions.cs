using System;
using System.Linq;
using System.Reflection;

namespace GQL.Services.Infra.Helpers
{
    internal static class CustomAttributeProviderExtensions
    {
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