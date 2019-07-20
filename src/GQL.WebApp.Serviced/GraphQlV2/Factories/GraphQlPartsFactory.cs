using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using GQL.WebApp.Serviced.GraphQlV2.Infra;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2.Factories
{
    public class GraphQlPartsFactory
    {
        public static readonly GraphQlPartsFactory Instance = new GraphQlPartsFactory();


        public FieldType CreateFieldType(PropertyInfo propertyInfo, IFieldResolver fieldResolver = null)
        {
            var type = propertyInfo.ResolveType(propertyInfo.PropertyType);

            return new FieldType
            {
                Name = propertyInfo.Name,
                Description = propertyInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DeprecationReason = propertyInfo.FindInAttributes<ObsoleteAttribute>()?.Message,
                Type = GraphQlUtils.GetGraphQlTypeFor(type, propertyInfo.IsNullable()),
                Arguments = null,
                Resolver = fieldResolver,
                DefaultValue = propertyInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public QueryArgument CreateQueryArgument(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.ResolveType(parameterInfo.ParameterType);

            return new QueryArgument(GraphQlUtils.GetGraphQlTypeFor(type, parameterInfo.IsNullable()))
            {
                Name = parameterInfo.Name,
                Description = parameterInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DefaultValue = parameterInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }

        public FieldType CreateFieldType(MethodInfo methodInfo, IEnumerable<QueryArgument> queryArguments, IFieldResolver fieldResolver = null)
        {
            var type = methodInfo.ResolveType(methodInfo.ReturnType);

            return new FieldType
            {
                Name = methodInfo.Name,
                Description = methodInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DeprecationReason = methodInfo.FindInAttributes<ObsoleteAttribute>()?.Message,
                Type = GraphQlUtils.GetGraphQlTypeFor(type, methodInfo.IsNullable()),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
                DefaultValue = methodInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }
    }
}