using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.WebApp.Serviced.GraphQlV2.Infra
{
    internal static class GraphQlUtils
    {
        public static IEnumerable<PropertyInfo> GetRegisteredProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => IsEnabledForRegister(p.PropertyType));
        }

        public static IEnumerable<MethodInfo> GetRegisteredMethods(Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => !m.IsSpecialName
                            && IsEnabledForRegister(m.ReturnType)
                            && m.DeclaringType != typeof(object));
        }

        public static IEnumerable<ParameterInfo> GetAvailableParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters()
                .Where(p => !IsResolveFieldContextType(p.ParameterType));
        }

        public static Type GetGraphQlTypeFor(Type type, bool isForceNullable)
        {
            var processingType = type.UnwrapTaskType();

            if (processingType.IsNullable())
            {
                processingType = processingType.GetGenericArguments()[0];
                isForceNullable = true;
            }

            Type graphQlType;
            if (processingType.IsArray)
            {
                var elementType = processingType.GetElementType();
                graphQlType = typeof(ListGraphType<>).MakeGenericType(
                    GetGraphQlTypeFor(elementType, elementType.IsNullable()));
            }
            else if (processingType.IsEnumerable())
            {
                var enumerableElementType = processingType.GetEnumerableElementType();
                graphQlType = typeof(ListGraphType<>).MakeGenericType(
                    GetGraphQlTypeFor(enumerableElementType, enumerableElementType.IsNullable()));
            }
            else
                graphQlType = GraphTypeTypeRegistry.Get(processingType);

            if (graphQlType == null)
                graphQlType = typeof(AutoRegisteringObjectGraphType<>).MakeGenericType(processingType);
            if (!isForceNullable)
                graphQlType = typeof(NonNullGraphType<>).MakeGenericType(graphQlType);
            return graphQlType;
        }

        public static bool IsEnabledForRegister(Type type)
        {
            return true;
            var realType = GetRealType(type);

            return GraphTypeTypeRegistry.Contains(realType);
        }

        public static Type GetRealType(Type type)
        {
            if (type.IsNullable())
                return type.GetGenericArguments()[0];
            if (type.IsArray)
                return type.GetElementType();
            if (type.IsEnumerable())
                return type.GetEnumerableElementType();
            if (type.IsTask())
                return type.UnwrapTaskType();
            return type;
        }

        public static bool IsResolveFieldContextType(Type type, params Type[] possibleTypes)
        {
            if (type == typeof(ResolveFieldContext))
            {
                return true;
            }

            if (type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>)))
            {
                if (possibleTypes.Length == 0)
                {
                    return true;
                }

                var typeArgument = type.GenericTypeArguments[0];
                return possibleTypes.Contains(typeArgument);
            }

            return false;
        }
    }
}