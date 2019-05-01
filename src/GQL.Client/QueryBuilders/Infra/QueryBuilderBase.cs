namespace GQL.Client.QueryBuilders.Infra
{
    public abstract class QueryBuilderBase
    {
        public abstract void ThrowIfNotValid();

        public abstract string Build();

        public override string ToString()
        {
            return Build();
        }
    }
}