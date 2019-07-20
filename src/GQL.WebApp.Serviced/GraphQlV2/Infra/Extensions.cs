using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GQL.WebApp.Serviced.GraphQlV2.Infra
{
    internal static class ReflectionExtensions
    {
        public static bool IsDefinedAttribute<T>(this ICustomAttributeProvider attributeProvider)
            where T : Attribute
        {
            return attributeProvider.IsDefined(typeof(T), false);
        }

        public static bool IsNullable(this ICustomAttributeProvider attributeProvider)
        {
            return !attributeProvider.IsDefinedAttribute<RequiredAttribute>();
        }

        public static Type ResolveType(this ICustomAttributeProvider attributeProvider, Type defaultType)
        {
            return attributeProvider.FindInAttributes<ReturnTypeAttribute>()?.ReturnType ?? defaultType;
        }

        public static T FindInAttributes<T>(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
        }
    }

    internal static class TypeExtensions
    {
        public static bool IsNullType(this Type type)
        {
            return !type.IsValueType || type.IsGenericTypeDefinition(typeof(Nullable<>));
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericTypeDefinition(typeof(Nullable<>));
        }

        public static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsTask(this Type type)
        {
            return type == typeof(Task) || type.IsGenericTypeDefinition(typeof(Task<>));
        }

        public static bool IsGenericTypeDefinition(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }

        public static Type GetEnumerableElementType(this Type type)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(type))
                throw new InvalidOperationException($"Type {type.Name} is not {nameof(IEnumerable)}");

            return type.IsGenericType
                ? type.GetGenericArguments()[0]
                : typeof(object);
        }

        public static Type UnwrapTaskType(this Type type)
        {
            var processingType = type;
            while (true)
            {
                if (processingType.IsGenericTypeDefinition(typeof(Task<>)))
                {
                    processingType = processingType.GenericTypeArguments[0];
                }
                else if (processingType == typeof(Task))
                {
                    return typeof(void);
                }
                else
                {
                    return processingType;
                }
            }
        }
    }
}