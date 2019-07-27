using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.FieldResolvers;
using GQL.Services.Infra.Helpers;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoObjectGraphType<T> : ObjectGraphType
    {
        private readonly GraphQlPartsFactory _graphQlPartsFactory = GraphQlPartsFactory.Instance;


        public AutoObjectGraphType()
        {
            var type = typeof(T);

            Name = type.GetNameOrDefault(type.Name);
            Description = type.GetDescription();
            DeprecationReason = type.GetDeprecationReason();

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(type))
            {
                AddField(_graphQlPartsFactory.CreateFieldType(
                    propertyInfo,
                    new PropertyFieldResolver(propertyInfo, ProviderContext.Instance)));
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(type))
            {
                AddField(_graphQlPartsFactory.CreateFieldType(
                    methodInfo,
                    new MethodFieldResolver(methodInfo, ProviderContext.Instance)));
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }
        }
    }
}