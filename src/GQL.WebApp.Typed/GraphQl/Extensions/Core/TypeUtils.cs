using System;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Extensions.Core
{
    public static class TypeUtils
    {
        public static bool IsResolveFieldContextType(Type type)
        {
            return type == typeof(ResolveFieldContext) || type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>));
        }

        public static Type UnwrapTaskType(Type type)
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