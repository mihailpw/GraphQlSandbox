using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.Services.Infra.Helpers
{
    internal static class GraphQlUtils
    {
        public const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;


        public static IEnumerable<PropertyInfo> GetRegisteredProperties(Type type)
        {
            return type.GetProperties(DefaultBindingFlags).Where(p => IsEnabledForRegister(p.PropertyType));
        }

        public static IEnumerable<MethodInfo> GetRegisteredMethods(Type type)
        {
            return type.GetMethods(DefaultBindingFlags).Where(m =>
                !m.IsSpecialName
                && IsEnabledForRegister(m.ReturnType)
                && m.DeclaringType != typeof(object));
        }

        public static IEnumerable<ParameterInfo> GetAvailableParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Where(p => !TypeUtils.ResolveFieldContext.IsInType(p.ParameterType));
        }

        public static Type GetGraphQlTypeFor(Type type, bool isForceNullable = false)
        {
            var processingType = TypeUtils.Task.UnwrapType(type);

            if (TypeUtils.Nullable.IsInType(processingType))
            {
                processingType = TypeUtils.Nullable.UnwrapType(processingType);
                isForceNullable = true;
            }

            Type graphQlType;
            if (TypeUtils.Enumerable.IsInType(processingType))
            {
                var enumerableElementType = TypeUtils.Enumerable.UnwrapType(processingType);
                var innerGraphQlType = GetGraphQlTypeFor(enumerableElementType);
                graphQlType = typeof(ListGraphType<>).MakeGenericType(innerGraphQlType);
            }
            else
            {
                graphQlType = typeof(AutoRegisteringObjectGraphType<>).MakeGenericType(processingType);
            }

            if (!isForceNullable)
            {
                graphQlType = typeof(NonNullGraphType<>).MakeGenericType(graphQlType);
            }

            return graphQlType;
        }

        public static bool IsEnabledForRegister(Type type)
        {
            var realType = TypeUtils.GetRealType(type);
            return GraphTypeTypeRegistry.Contains(realType);
        }
    }

    public static class TypeUtils
    {
        public static class Enumerable
        {
            public static bool IsInType(Type type)
            {
                return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
            }

            public static Type UnwrapType(Type type)
            {
                var processingType = type;
                while (true)
                {
                    if (processingType == typeof(string))
                    {
                        return processingType;
                    }

                    if (processingType.IsGenericTypeDefinition(typeof(IEnumerable<>)))
                    {
                        processingType = processingType.GenericTypeArguments[0];
                    }
                    else if (processingType == typeof(IEnumerable))
                    {
                        return typeof(object);
                    }
                    else
                    {
                        return processingType;
                    }
                }
            }
        }

        public static class Task
        {
            public static bool IsInType(Type type)
            {
                return type == typeof(Task) || type.IsGenericTypeDefinition(typeof(Task<>));
            }

            public static Type UnwrapType(Type type)
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

        public static class Nullable
        {
            public static bool IsInType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Nullable<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(Nullable<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }

        public static class ResolveFieldContext
        {
            public static bool IsInType(Type type)
            {
                return type == typeof(ResolveFieldContext) || type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>));
            }

            public static Type UnwrapType(Type type)
            {
                return type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>))
                    ? type.GenericTypeArguments[0]
                    : type;
            }
        }


        public static Type GetRealType(Type type)
        {
            if (Nullable.IsInType(type))
                return Nullable.UnwrapType(type);
            if (Enumerable.IsInType(type))
                return Enumerable.UnwrapType(type);
            if (Task.IsInType(type))
                return Task.UnwrapType(type);
            return type;
        }
    }

    public class ConvertUtils
    {
        public static object ChangeTypeTo(object value, Type toType)
        {
            if (value == null)
            {
                return null;
            }

            var underlyingType = toType;

            if (TypeUtils.Nullable.IsInType(underlyingType))
            {
                var converter = new NullableConverter(underlyingType);
                underlyingType = converter.UnderlyingType;
            }

            if (underlyingType == typeof(Guid))
            {
                return new Guid(value.ToString());
            }

            // ReSharper disable once UseMethodIsInstanceOfType
            return underlyingType.IsAssignableFrom(value.GetType())
                ? Convert.ChangeType(value, underlyingType)
                : Convert.ChangeType(value.ToString(), underlyingType);
        }

        public static object ChangeResolveFieldContextTypeTo(ResolveFieldContext context, Type sourceType)
        {
            return Activator.CreateInstance(typeof(ResolveFieldContext<>).MakeGenericType(sourceType), context);
        }
    }
}