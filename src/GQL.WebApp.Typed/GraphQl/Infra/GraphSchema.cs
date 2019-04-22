using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Infra
{
    public abstract class GraphSchema : Schema
    {
        protected GraphSchema(GraphQuery query, GraphMutation mutation = null, GraphSubscription subscription = null)
        {
            Query = query;
            Mutation = mutation;
            Subscription = subscription;
        }
    }
}