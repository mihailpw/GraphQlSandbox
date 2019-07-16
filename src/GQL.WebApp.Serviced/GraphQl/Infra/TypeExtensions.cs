using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

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

        public static bool CheckIfResolveFieldContextType(this Type type, params Type[] possibleTypes)
        {
            if (type == typeof(ResolveFieldContext)
                || type == typeof(ResolveFieldContext<object>))
            {
                return true;
            }

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(ResolveFieldContext<>))
                {
                    if (possibleTypes.Length == 0)
                    {
                        return true;
                    }

                    var typeArgument = type.GenericTypeArguments[0];
                    return possibleTypes.Contains(typeArgument);

                }
            }

            return false;
        }
    }
}