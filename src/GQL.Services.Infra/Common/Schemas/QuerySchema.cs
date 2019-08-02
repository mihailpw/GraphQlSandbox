using GQL.Services.Infra.Common.Helpers;

namespace GQL.Services.Infra.Common.Schemas
{
    internal sealed class QuerySchema<TQuery> : SchemaBase
    {
        public QuerySchema(IGraphQlTypeRegistry typeRegistry)
            : base(typeRegistry)
        {
            Query = ActivatorHelper.CreateGraphQlObject(typeof(TQuery));

            PopulateAdditionalTypes();
        }
    }
}