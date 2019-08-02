using GQL.Services.Infra.Common.Helpers;

namespace GQL.Services.Infra.Common.Schemas
{
    internal class QueryMutationSubscriptionSchema<TQuery, TMutation, TSubscription> : SchemaBase
    {
        public QueryMutationSubscriptionSchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));
            Mutation = ActivatorHelper.CreateGraphQlObject(typeof(TMutation));
            Subscription = ActivatorHelper.CreateGraphQlObject(typeof(TSubscription));

            PopulateAdditionalTypes();
        }
    }
}