using System;
using System.ComponentModel;
using System.Linq;
using GQL.WebApp.Serviced.GraphQl.Infra.Resolvers;
using GQL.WebApp.Serviced.GraphQlV2.Infra;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2.Factories
{
    public class ObjectGraphTypeFactory
    {
        public IObjectGraphType CreateObject(Type type)
        {
            var objectGraphType = ActivatorHelper.CreateInstance<IObjectGraphType>(typeof(AutoRegisteringObjectGraphType<>), type);

            return objectGraphType;
        }
    }
}