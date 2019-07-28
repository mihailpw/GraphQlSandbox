using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.Core
{
    internal class GraphQlPartsFactory : IGraphQlPartsFactory
    {
        private readonly IGraphQlTypeRegistry _typeRegistry;


        public GraphQlPartsFactory(IGraphQlTypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
        }


        public FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null)
        {
            var type = propertyInfo.GetReturnTypeOrDefault(propertyInfo.PropertyType);

            return new FieldType
            {
                Name = propertyInfo.GetNameOrDefault(propertyInfo.Name),
                Description = propertyInfo.GetDescription(),
                DeprecationReason = propertyInfo.GetDeprecationReason(),
                Type = GraphQlUtils.GetGraphQlTypeFor(type, propertyInfo.IsRequired()),
                Arguments = null,
                Resolver = fieldResolver,
                DefaultValue = propertyInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public FieldType CreateFieldType(MethodInfo methodInfo, IFieldResolver fieldResolver = null)
        {
            var type = methodInfo.GetReturnTypeOrDefault(methodInfo.ReturnType);
            var queryArguments = GraphQlUtils.GetAvailableParameters(methodInfo).Select(CreateQueryArgument);

            return new FieldType
            {
                Name = methodInfo.GetNameOrDefault(methodInfo.Name),
                Description = methodInfo.GetDescription(),
                DeprecationReason = methodInfo.GetDeprecationReason(),
                Type = GraphQlUtils.GetGraphQlTypeFor(type, methodInfo.IsRequired()),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
                DefaultValue = methodInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public Type CreateInterfaceType(Type interfaceType)
        {
            return GraphQlUtils.GetGraphQlTypeFor(interfaceType);
        }

        public Func<object, bool> CreateIsTypeOfFunc(Type type)
        {
            var additionalTypes = _typeRegistry.ResolveAdditional(type).ToList();

            bool IsTypeOfFunc(object target)
            {
                var targetType = target.GetType();
                if (type == targetType)
                {
                    return true;
                }

                foreach (var additionalType in additionalTypes)
                {
                    if (additionalType == targetType)
                    {
                        return true;
                    }
                }

                return false;
            }

            return IsTypeOfFunc;
        }


        private static QueryArgument CreateQueryArgument(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.GetReturnTypeOrDefault(parameterInfo.ParameterType);

            return new QueryArgument(GraphQlUtils.GetGraphQlTypeFor(type, parameterInfo.IsRequired()))
            {
                Name = parameterInfo.Name,
                Description = parameterInfo.GetDescription(),
                DefaultValue = parameterInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }
    }
}