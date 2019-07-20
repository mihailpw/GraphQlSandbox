using System;
using System.ComponentModel;
using System.Linq;
using GQL.WebApp.Serviced.GraphQl.Infra.Resolvers;
using GQL.WebApp.Serviced.GraphQlV2.Factories;
using GQL.WebApp.Serviced.GraphQlV2.Infra;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2
{
    public sealed class AutoRegisteringObjectGraphType<TSourceType> : ObjectGraphType<TSourceType>
    {
        private readonly GraphQlPartsFactory _graphQlPartsFactory = GraphQlPartsFactory.Instance;


        public AutoRegisteringObjectGraphType()
        {
            var serviceType = typeof(TSourceType);

            Name = serviceType.Name;
            Description = serviceType.FindInAttributes<DescriptionAttribute>()?.Description;
            DeprecationReason = serviceType.FindInAttributes<ObsoleteAttribute>()?.Message;

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(typeof(TSourceType)))
            {
                AddField(_graphQlPartsFactory.CreateFieldType(
                    propertyInfo,
                    new PropertyFieldResolver(serviceType, propertyInfo, ProviderContext.Instance)));
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(TSourceType)))
            {
                var type = methodInfo.ResolveType(methodInfo.ReturnType);
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