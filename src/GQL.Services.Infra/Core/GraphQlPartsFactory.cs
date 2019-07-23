using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.Core
{
    public class GraphQlPartsFactory
    {
        public static readonly GraphQlPartsFactory Instance = new GraphQlPartsFactory();


        public FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null)
        {
            var type = propertyInfo.GetReturnTypeOrDefault(propertyInfo.PropertyType);

            return new FieldType
            {
                Name = propertyInfo.GetNameOrDefault(propertyInfo.Name),
                Description = propertyInfo.GetDescription(),
                DeprecationReason = propertyInfo.GetDeprecationReason(),
                Type = GraphQlUtils.GetGraphQlTypeFor(type),
                Arguments = null,
                Resolver = fieldResolver,
                DefaultValue = propertyInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public QueryArgument CreateQueryArgument(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.GetReturnTypeOrDefault(parameterInfo.ParameterType);

            return new QueryArgument(GraphQlUtils.GetGraphQlTypeFor(type))
            {
                Name = parameterInfo.Name,
                Description = parameterInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DefaultValue = parameterInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public FieldType CreateFieldType(MethodInfo methodInfo, IEnumerable<QueryArgument> queryArguments, IFieldResolver fieldResolver = null)
        {
            var type = methodInfo.GetReturnTypeOrDefault(methodInfo.ReturnType);

            return new FieldType
            {
                Name = methodInfo.Name,
                Description = methodInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DeprecationReason = methodInfo.FindInAttributes<ObsoleteAttribute>()?.Message,
                Type = GraphQlUtils.GetGraphQlTypeFor(type),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
                DefaultValue = methodInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }
    }
}