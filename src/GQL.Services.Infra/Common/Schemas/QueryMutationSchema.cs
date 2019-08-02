using GQL.Services.Infra.Common.Helpers;

namespace GQL.Services.Infra.Common.Schemas
{
    internal sealed class QueryMutationSchema<TQuery, TMutation> : SchemaBase
    {
        public QueryMutationSchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));
            Mutation = ActivatorHelper.CreateGraphQlObject(typeof(TMutation));

            PopulateAdditionalTypes();
        }
    }
}