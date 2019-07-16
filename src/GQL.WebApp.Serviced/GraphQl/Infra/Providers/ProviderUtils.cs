using System;
using System.Linq;
using System.Reflection;
using GraphQL;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Providers
{
    internal static class ProviderUtils
    {
        public static bool TryGetType(MemberInfo memberInfo, out Type type)
        {
            type = memberInfo.FindInAttributes<ITypeHolder>()?.Type;
            return type != null;
        }

        public static bool CheckIfInterfaceType(Type type)
        {
            return type.IsInterface || type.IsAbstract || type.FindInAttributes<IInterfaceHolder>()?.IsInterface == true;
        }

        public static string GetName(MemberInfo memberInfo)
        {
            return memberInfo.FindInAttributes<INameProvider>()?.Name ?? memberInfo.Name.ToCamelCase();
        }

        public static string GetName(ParameterInfo parameterInfo)
        {
            return parameterInfo.FindInAttributes<INameProvider>()?.Name ?? parameterInfo.Name.ToCamelCase();
        }

        public static Type GetReturnType(MethodInfo methodInfo)
        {
            return methodInfo.FindInAttributes<IReturnTypeProvider>()?.ReturnType ?? methodInfo.ReturnType;
        }

        public static Type GetReturnType(ParameterInfo parameterInfo)
        {
            return parameterInfo.FindInAttributes<IReturnTypeProvider>()?.ReturnType ?? parameterInfo.ParameterType;
        }

        public static string GetDescription(ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<IDescriptionProvider>()?.Description;
        }

        public static string GetDeprecationReason(ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.FindInAttributes<IDeprecationReasonProvider>()?.DeprecationReason;
        }

        private static T FindInAttributes<T>(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
        }
    }
}