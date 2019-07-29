using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.FieldResolvers;
using GQL.Services.Infra.Helpers;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoObjectGraphType<T> : ObjectGraphType<T>
    {
        private readonly IGraphQlPartsFactory _partsFactory = GlobalContext.PartsFactory;
        private readonly IConfig _config = GlobalContext.Config;
        private readonly IScopedProvider _scopedProvider = GlobalContext.ScopedProvider;


        public AutoObjectGraphType()
        {
            var type = typeof(T);

            Name = type.GetNameOrDefault(type.Name);
            Description = type.GetDescription();
            DeprecationReason = type.GetDeprecationReason();

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(type))
            {
                AddField(_partsFactory.CreateFieldType(
                    propertyInfo,
                    new PropertyFieldResolver(propertyInfo, _config)));
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(type))
            {
                AddField(_partsFactory.CreateFieldType(
                    methodInfo,
                    new MethodFieldResolver(type, methodInfo, _scopedProvider)));
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }

            var requireConfigureIsTypeOf = false;
            foreach (var interfaceType in GraphQlUtils.GetRegisteredInterfaces(type))
            {
                requireConfigureIsTypeOf = true;
                Interface(_partsFactory.CreateInterfaceType(interfaceType));
            }

            if (requireConfigureIsTypeOf)
            {
                IsTypeOf = _partsFactory.CreateIsTypeOfFunc(type);
            }
        }
    }
}