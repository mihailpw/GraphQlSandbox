using System;
using System.Linq;
using System.Reflection;
using GraphQL;
using GraphQL.Reflection;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2._
{
    internal static class ReflectionHelper
    {
        public static IAccessor ToAccessor(this Type type, string field, ResolverType resolverType)
        {
            if (type == null)
                return null;
            var getter1 = type.MethodForField(field, resolverType);
            if (getter1 != null)
                return new SingleMethodAccessor(getter1);
            if (resolverType != ResolverType.Resolver)
                return null;
            var getter2 = type.PropertyForField(field);
            if (getter2 != null)
                return new SinglePropertyAccessor(getter2);
            return null;
        }

        public static MethodInfo MethodForField(
          this Type type,
          string field,
          ResolverType resolverType)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(m =>
            {
                var customAttribute = m.GetCustomAttribute<GraphQLMetadataAttribute>();
                if (string.Equals(field, customAttribute?.Name ?? m.Name, StringComparison.OrdinalIgnoreCase))
                    return resolverType == (customAttribute?.Type ?? ResolverType.Resolver);
                return false;
            });
        }

        public static PropertyInfo PropertyForField(this Type type, string field)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => string.Equals(
                    field,
                    m.GetCustomAttribute<GraphQLMetadataAttribute>()?.Name ?? m.Name,
                    StringComparison.OrdinalIgnoreCase));
        }

        public static object[] BuildArguments<T>(ParameterInfo[] parameters, T context) where T : ResolveFieldContext<object>
        {
            if (parameters == null || parameters.Length == 0)
                return null;
            var objArray = new object[parameters.Length];
            var count = 0;
            if (typeof(T) == parameters[count].ParameterType)
            {
                objArray[count] = context;
                ++count;
            }
            if (parameters.Length > count && context.Source != null && (context.Source?.GetType() == parameters[count].ParameterType || string.Equals(parameters[count].Name, "source", StringComparison.OrdinalIgnoreCase)))
            {
                objArray[count] = context.Source;
                ++count;
            }
            if (parameters.Length > count && context.UserContext != null && context.UserContext?.GetType() == parameters[count].ParameterType)
            {
                objArray[count] = context.UserContext;
                ++count;
            }
            foreach (var parameterInfo in parameters.Skip(count))
            {
                objArray[count] = context.GetArgument(parameterInfo.ParameterType, parameterInfo.Name, null);
                ++count;
            }
            return objArray;
        }
    }
}