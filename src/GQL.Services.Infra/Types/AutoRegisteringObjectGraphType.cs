using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.FieldResolvers;
using GQL.Services.Infra.Helpers;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    public sealed class AutoRegisteringObjectGraphType<TSourceType> : ObjectGraphType<TSourceType>
    {
        private readonly GraphQlPartsFactory _graphQlPartsFactory = GraphQlPartsFactory.Instance;


        public AutoRegisteringObjectGraphType()
        {
            var serviceType = typeof(TSourceType);

            Name = serviceType.GetNameOrDefault(serviceType.Name);
            Description = serviceType.GetDescription();
            DeprecationReason = serviceType.GetDeprecationReason();

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(typeof(TSourceType)))
            {
                AddField(_graphQlPartsFactory.CreateFieldType(
                    propertyInfo,
                    new PropertyFieldResolver(serviceType, propertyInfo, ProviderContext.Instance)));
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(TSourceType)))
            {
                var type = methodInfo.GetReturnTypeOrDefault(methodInfo.ReturnType);
                var queryArguments = GraphQlUtils.GetAvailableParameters(methodInfo).Select(_graphQlPartsFactory.CreateQueryArgument);

                AddField(_graphQlPartsFactory.CreateFieldType(
                    methodInfo,
                    queryArguments,
                    new MethodFieldResolver(typeof(TSourceType), methodInfo.ReturnType, type, methodInfo, ProviderContext.Instance)));
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {typeof(object).Name} has not supported fields.");
            }
        }

    }
}