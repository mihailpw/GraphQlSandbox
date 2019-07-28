using System;
using System.Linq;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Helpers;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoInterfaceGraphType<T> : InterfaceGraphType
    {
        private readonly IGraphQlPartsFactory _partsFactory = GlobalContext.PartsFactory;


        public AutoInterfaceGraphType()
        {
            var type = typeof(T);

            Name = type.GetNameOrDefault(type.Name);
            Description = type.GetDescription();
            DeprecationReason = type.GetDeprecationReason();

            foreach (var propertyInfo in GraphQlUtils.GetRegisteredProperties(typeof(T)))
            {
                AddField(_partsFactory.CreateFieldType(propertyInfo));
            }

            foreach (var methodInfo in GraphQlUtils.GetRegisteredMethods(typeof(T)))
            {
                AddField(_partsFactory.CreateFieldType(methodInfo));
            }

            if (!Fields.Any())
            {
                throw new InvalidOperationException($"Type {type.Name} has not supported fields.");
            }
        }
    }
}