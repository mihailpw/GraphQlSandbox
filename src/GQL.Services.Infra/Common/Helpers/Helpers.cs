using System;
using GQL.Services.Infra.Common.Types;
using GraphQL.Types;

namespace GQL.Services.Infra.Common.Helpers
{
    internal static class ActivatorHelper
    {
        public static IObjectGraphType CreateGraphQlObject(Type type)
        {
            return CreateInstance<IObjectGraphType>(typeof(AutoObjectGraphType<>), type);
        }

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