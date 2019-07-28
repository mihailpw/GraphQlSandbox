using System;
using System.Linq;
using GQL.Services.Infra;
using GQL.Services.Infra.Core;
using GQL.WebApp.Serviced.GraphQlV2;
using GQL.WebApp.Serviced.GraphQlV2.Models;
using GraphQL.Types;

namespace GQL.WebApp.Serviced
{
    public class GraphQlSchema : Schema
    {
        private readonly IGraphQlTypeRegistry _typeRegistry;

        public GraphQlSchema(IGraphQlTypeRegistry typeRegistry)
        {
            _typeRegistry = typeRegistry;
            //var res = new GraphTypeFactory(provider).Create(typeof(GqlUsersQueryType));
            //Query = res as IObjectGraphType;
            //GraphTypeTypeRegistry.Register<GraphQlAutoRegisteringModelType2.Model2, GraphQlAutoRegisteringModelType2>();
            //Query = new GraphQlAutoRegisteringModelType();

            Query = new GraphTypeFactory().CreateObject(typeof(UsersQuery));

            RegisterType(typeof(CustomerUserObject));
            RegisterType(typeof(ManagerUserObject));
        }

        public void RegisterType(Type type)
        {
            var graphQlType = _typeRegistry.Resolve(type);
            var methodInfo = GetType().GetMethods().First(mi => mi.Name == nameof(RegisterType) && mi.IsGenericMethod);
            methodInfo.MakeGenericMethod(graphQlType).Invoke(this, new object[0]);
        }
    }
}