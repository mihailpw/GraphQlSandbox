using System.Collections.Generic;
using GQL.WebApp.Typed.GraphQl.Infra;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas
{
    public class AppSchema : Schema
    {
        public AppSchema(
            IEnumerable<GraphQuery> graphQueries,
            IEnumerable<GraphMutation> graphMutations,
            IEnumerable<GraphSubscription> graphSubscriptions)
        {
            Query = new CompositeObjectGraphType(graphQueries);
            Mutation = new CompositeObjectGraphType(graphMutations);
            Subscription = new CompositeObjectGraphType(graphSubscriptions);
        }
    }
}