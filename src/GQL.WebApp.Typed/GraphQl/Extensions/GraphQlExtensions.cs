using System;
using System.Collections.Generic;
using System.Reflection;
using GQL.WebApp.Typed.GraphQl.Extensions.Core;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.WebApp.Typed.GraphQl.Extensions
{
    public static class GraphQlExtensions
    {
        public static FieldTypeBuilder MethodField<T>(this IComplexGraphType graphType, string name, string methodName)
            where T : IGraphType
        {
            var type = graphType.GetType();
            var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(methodName), methodName, $"Method '{methodName}' in type {graphType.GetType().Name} not found.");
            }

            var arguments = CreateArguments(methodInfo);
            var returnType = typeof(T);
            //var returnType = GraphTypeTypeRegistry.Get(TypeUtils.UnwrapTaskType(methodInfo.ReturnType));
            //if (returnType == null)
            //{
            //    throw new InvalidOperationException($"Return type of method {methodInfo.Name} ({methodInfo.DeclaringType?.Name}) should be specified.");
            //}

            var fieldType = new FieldType
            {
                Name = name,
                Arguments = new QueryArguments(arguments),
                Type = returnType,
                Resolver = new MethodFieldResolver(graphType, methodInfo),
            };
            graphType.AddField(fieldType);

            return new FieldTypeBuilder(fieldType);
        }


        private static IEnumerable<QueryArgument> CreateArguments(MethodBase methodInfo)
        {
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var queryArgumentAttribute = parameterInfo.GetCustomAttribute<QueryArgumentAttribute>();
                if (queryArgumentAttribute == null)
                {
                    continue;
                }

                var name = queryArgumentAttribute.Name ?? parameterInfo.Name;
                var type = queryArgumentAttribute.Type ?? GraphTypeTypeRegistry.Get(parameterInfo.ParameterType);

                if (type == null)
                {
                    throw new InvalidOperationException($"Type of parameter {parameterInfo.Name} in {methodInfo.Name} method ({methodInfo.DeclaringType?.Name}) should be specified.");
                }

                var queryArgument = new QueryArgument(type)
                {
                    Name = parameterInfo.Name,
                    DefaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null
                };

                yield return queryArgument;
            }
        }
    }
}