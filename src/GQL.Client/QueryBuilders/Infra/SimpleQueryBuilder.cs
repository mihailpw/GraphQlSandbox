namespace GQL.Client.QueryBuilders.Infra
{
    public class SimpleQueryBuilder : QueryBuilderBase
    {
        private readonly string _key;


        public SimpleQueryBuilder(string key)
        {
            _key = key;
        }


        public override void ThrowIfNotValid()
        {
        }

        public override string Build()
        {
            return _key;
        }
    }
}