using System;
using System.Linq;
using System.Reflection;

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
            return memberInfo.FindInAttributes<INameProvider>()?.Name ?? memberInfo.Name;
        }

        public static string GetName(ParameterInfo parameterInfo)
        {
            return parameterInfo.FindInAttributes<INameProvider>()?.Name ?? parameterInfo.Name;
        }

        public static string GetDescription(MemberInfo memberInfo)
        {
            return memberInfo.FindInAttributes<IDescriptionProvider>()?.Description;
        }

        public static string GetDeprecationReason(MemberInfo memberInfo)
        {
            return memberInfo.FindInAttributes<IDeprecationReasonProvider>()?.DeprecationReason;
        }

        private static T FindInAttributes<T>(this ICustomAttributeProvider attributeProvider)
        {
            return attributeProvider.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
        }
    }
}