using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Helpers;
using GQL.Services.Infra.Providers;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoInterfaceGraphType<T> : InterfaceGraphType
    {
        public AutoInterfaceGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;
            var partsFactory = GlobalContext.PartsFactory;

            var type = typeof(T);
            if (!type.IsGraphQlMember())
            {
                throw new InvalidOperationException($"Type {type.Name} should be marked with {nameof(GraphQLAttribute)}.");
            }

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(type))
            {
                var fieldType = partsFactory.CreateFieldType(propertyInfo);
                propertyInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, propertyInfo, serviceProvider);
                AddField(fieldType);
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(T)))
            {
                var fieldType = partsFactory.CreateFieldType(methodInfo);
                methodInfo.FindInAttributes<IFieldTypeInfoProvider>()?.Provide(fieldType, methodInfo, serviceProvider);
                AddField(fieldType);
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }
        }
    }
}