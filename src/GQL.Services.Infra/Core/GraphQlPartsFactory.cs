using System;
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
            return new FieldType
            {
                Name = propertyInfo.Name,
                Type = GraphQlUtils.GetGraphQlTypeFor(propertyInfo.PropertyType),
                Arguments = null,
                Resolver = fieldResolver,
            };
        }

        public FieldType CreateFieldType(MethodInfo methodInfo, IFieldResolver fieldResolver = null)
        {
            var queryArguments = GraphQlUtils.GetAvailableParameters(methodInfo).Select(CreateQueryArgument);

            return new FieldType
            {
                Name = methodInfo.Name,
                Type = GraphQlUtils.GetGraphQlTypeFor(methodInfo.ReturnType),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
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
            var defaultValue = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null;

            return new QueryArgument(GraphQlUtils.GetGraphQlTypeFor(parameterInfo.ParameterType))
            {
                Name = parameterInfo.Name,
                DefaultValue = defaultValue
            };
        }
    }
}