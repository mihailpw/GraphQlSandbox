using System;
using System.Collections;
using System.Threading.Tasks;

namespace GQL.WebApp.Serviced.GraphQl.Infra
{
    public static class TypeExtensions
    {
        public static bool CheckIfEnumerable(this Type type)
        {
            return type != typeof(string)
                   && !type.IsArray
                   && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool CheckIfNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool CheckIfTask(this Type type)
        {
            return type == typeof(Task)
                   || type.IsGenericType
                   && type.GetGenericTypeDefinition() == typeof(Task<>);
        }

        public static Type UnwrapTypeFromTask(this Type type)
        {
            var processingType = type;
            while (true)
            {
                if (processingType.IsGenericType && processingType.GetGenericTypeDefinition() == typeof(Task<>))
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

        public static Type GetGenericTypeDefinitionSafe(this Type type)
        {
            return type.IsGenericTypeDefinition
                ? type.GetGenericTypeDefinition()
                : typeof(void);
        }
    }
}