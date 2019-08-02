using GQL.Services.Infra.Common.Helpers;

namespace GQL.Services.Infra.Common.Schemas
{
    internal sealed class QuerySubscriptionSchema<TQuery, TSubscription> : SchemaBase
    {
        public QuerySubscriptionSchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));
            Subscription = ActivatorHelper.CreateGraphQlObject(typeof(TSubscription));

            PopulateAdditionalTypes();
        }
    }
}