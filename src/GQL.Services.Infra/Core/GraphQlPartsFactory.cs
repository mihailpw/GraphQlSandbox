using System;
using System.ComponentModel;
using System.Linq;
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
                Name = methodInfo.Name,
                Description = methodInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DeprecationReason = methodInfo.FindInAttributes<ObsoleteAttribute>()?.Message,
                Type = GraphQlUtils.GetGraphQlTypeFor(type, methodInfo.IsRequired()),
                Arguments = new QueryArguments(queryArguments),
                Resolver = fieldResolver,
                DefaultValue = methodInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }


        private static QueryArgument CreateQueryArgument(ParameterInfo parameterInfo)
        {
            var type = parameterInfo.GetReturnTypeOrDefault(parameterInfo.ParameterType);

            return new QueryArgument(GraphQlUtils.GetGraphQlTypeFor(type, parameterInfo.IsRequired()))
            {
                Name = parameterInfo.Name,
                Description = parameterInfo.FindInAttributes<DescriptionAttribute>()?.Description,
                DefaultValue = parameterInfo.FindInAttributes<DefaultValueAttribute>()?.Value,
            };
        }
    }
}