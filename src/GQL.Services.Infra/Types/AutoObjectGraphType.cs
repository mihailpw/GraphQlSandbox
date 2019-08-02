using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.FieldResolvers;
using GQL.Services.Infra.Helpers;
using GQL.Services.Infra.Providers;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoObjectGraphType<T> : ObjectGraphType<T>
    {
        public AutoObjectGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;
            var partsFactory = GlobalContext.PartsFactory;
            var config = GlobalContext.Config;

            var type = typeof(T);
            if (!type.IsGraphQlMember())
            {
                throw new InvalidOperationException($"Type {type.Name} should be marked with {nameof(GraphQLAttribute)}.");
            }

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);

            var s = GraphQlUtils.GetRegisteredProperties(type).ToList();
            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(type))
            {
                var fieldType = partsFactory.CreateFieldType(propertyInfo, new PropertyFieldResolver(propertyInfo, config));
                propertyInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, propertyInfo, serviceProvider);
                AddField(fieldType);
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(T)))
            {
                var fieldType = partsFactory.CreateFieldType(methodInfo, new MethodFieldResolver(type, methodInfo, serviceProvider));
                methodInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, methodInfo, serviceProvider);
                AddField(fieldType);
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }

            var requireConfigureIsTypeOf = false;
            foreach (var interfaceType in GraphQlUtils.GetRegisteredInterfaces(type))
            {
                requireConfigureIsTypeOf = true;
                Interface(partsFactory.CreateInterfaceType(interfaceType));
            }

            if (requireConfigureIsTypeOf)
            {
                IsTypeOf = partsFactory.CreateIsTypeOfFunc(type);
            }
        }
    }
}