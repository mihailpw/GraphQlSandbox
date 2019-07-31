using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GQL.Services.Infra.Core;
using GraphQL.Types;

namespace GQL.Services.Infra.Helpers
{
    internal static class GraphQlUtils
    {
        public const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;


        public static IEnumerable<PropertyInfo> GetRegisteredProperties(Type type)
        {
            bool CheckIfPropertySupported(PropertyInfo propertyInfo)
            {
                var isSupported = propertyInfo.IsGraphQlMember();
                if (!isSupported)
                {
                    return false;
                }

                var propertyType = propertyInfo.GetReturnTypeOrDefault(propertyInfo.PropertyType);
                if (!IsEnabledForRegister(propertyType))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    throw new NotSupportedException($"The property type of {propertyInfo.DeclaringType.Name}.{propertyInfo.Name} is not registered (type name: {propertyType.Name}).");
                }

                return true;
            }

            return type.GetProperties(DefaultBindingFlags).Where(CheckIfPropertySupported);
        }

        public static IEnumerable<MethodInfo> GetRegisteredMethods(Type type)
        {
            bool CheckIfMethodSupported(MethodInfo methodInfo)
            {
                var isSupported = !methodInfo.IsSpecialName
                    && methodInfo.IsGraphQlMember()
                    && methodInfo.DeclaringType != typeof(object);
                if (!isSupported)
                {
                    return false;
                }

                var returnType = methodInfo.GetReturnTypeOrDefault(methodInfo.ReturnType);
                if (!IsEnabledForRegister(returnType))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    throw new NotSupportedException($"The return type of {methodInfo.DeclaringType.Name}.{methodInfo.Name}() is not registered (type name: {returnType.Name}).");
                }

                return true;
            }

            return type.GetMethods(DefaultBindingFlags).Where(CheckIfMethodSupported);
        }

        public static IEnumerable<Type> GetRegisteredInterfaces(Type type)
        {
            return type.GetInterfaces().Where(t => t.IsGraphQlMember() && IsEnabledForRegister(t));
        }

        public static IEnumerable<ParameterInfo> GetAvailableParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().Where(p => !TypeUtils.ResolveFieldContext.IsInType(p.ParameterType));
        }

        public static Type GetGraphQlTypeFor(Type type, bool isRequired = false)
        {
            var processingType = TypeUtils.Task.UnwrapType(type);
            var isNullable = !processingType.IsValueType;

            if (TypeUtils.Nullable.IsInType(processingType))
            {
                processingType = TypeUtils.Nullable.UnwrapType(processingType);
                isNullable = true;
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
                graphQlType = GlobalContext.TypeRegistry.Resolve(processingType);
            }

            if (isRequired || !isNullable)
            {
                graphQlType = typeof(NonNullGraphType<>).MakeGenericType(graphQlType);
            }

            return graphQlType;
        }

        public static bool IsEnabledForRegister(Type type)
        {
            var realType = TypeUtils.GetRealType(type);
            var isEnabledForRegister = GlobalContext.TypeRegistry.IsRegistered(realType);
            return isEnabledForRegister;
        }

        public static IEnumerable<object> BuildArguments(
            MethodInfo methodInfo,
            ResolveFieldContext context)
        {
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var returnType = parameterInfo.ParameterType;
                if (TypeUtils.ResolveFieldContext.IsInType(returnType))
                {
                    if (returnType.IsGenericType)
                    {
                        var contextSourceType = returnType.GenericTypeArguments[0];
                        if (contextSourceType.IsInstanceOfType(context.Source))
                        {
                            yield return Activator.CreateInstance(typeof(ResolveFieldContext<>).MakeGenericType(contextSourceType), context);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Provided context with {contextSourceType.Name} type can not be processed. Context type can be only with {context.Source.GetType().Name} type.");
                        }
                    }
                    else
                    {
                        yield return context;
                    }
                }
                else
                {
                    var value = context.GetArgument(returnType, parameterInfo.GetNameOrDefault(parameterInfo.Name));
                    yield return value;
                }
            }

        }
    }

    internal static class TypeUtils
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

                    var interfaces = processingType.GetInterfaces().Append(processingType).ToArray();
                    if (interfaces.Any(i => i.IsGenericTypeDefinition(typeof(IEnumerable<>))))
                    {
                        processingType = processingType.GenericTypeArguments[0];
                    }
                    else if (interfaces.Any(i => i == typeof(IEnumerable)))
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
                return type == typeof(GraphQL.Types.ResolveFieldContext) || type.IsGenericTypeDefinition(typeof(ResolveFieldContext<>));
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
            var resultType = type;
            while (true)
            {
                if (Nullable.IsInType(resultType))
                    resultType = Nullable.UnwrapType(resultType);
                else if (Enumerable.IsInType(resultType))
                    resultType = Enumerable.UnwrapType(resultType);
                else if (Task.IsInType(resultType))
                    resultType = Task.UnwrapType(resultType);
                else
                    return resultType;
            }
        }
    }
}