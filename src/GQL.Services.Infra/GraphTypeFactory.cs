using System;
using GQL.Services.Infra.Helpers;
using GQL.Services.Infra.Types;
using GraphQL.Types;

namespace GQL.Services.Infra
{
    public class GraphTypeFactory
    {
        public IObjectGraphType CreateObject(Type type)
        {
            var objectGraphType = ActivatorHelper.CreateInstance<IObjectGraphType>(
                typeof(AutoObjectGraphType<>),
                type);

            return objectGraphType;
        }
    }
}