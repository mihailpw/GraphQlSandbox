using GQL.WebApp.Serviced.GraphQlV2;
using GQL.WebApp.Serviced.GraphQlV2.Factories;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.WebApp.Serviced
{
    public class GraphQlSchema : Schema
    {
        public GraphQlSchema(IProvider provider)
        {
            //var res = new ObjectGraphTypeFactory(provider).Create(typeof(GqlUsersQueryType));
            //Query = res as IObjectGraphType;
            //GraphTypeTypeRegistry.Register<GraphQlAutoRegisteringModelType2.Model2, GraphQlAutoRegisteringModelType2>();
            //Query = new GraphQlAutoRegisteringModelType();

            Query = new ObjectGraphTypeFactory().CreateObject(typeof(QueryRootService));
        }
    }
}