using System;

namespace GQL.WebApp.Serviced.GraphQlV2.Infra
{
    public static class ActivatorHelper
    {
        public static T CreateInstance<T>(Type type, params Type[] typeArguments)
        {
            return (T) Activator.CreateInstance(type.MakeGenericType(typeArguments));
        }
    }
}