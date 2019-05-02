namespace GQL.Client.QueryBuilders
{
    public class AppClient
    {
        private readonly string _url;


        public AppClient(string url)
        {
            _url = url;
        }


        public IQueryBuilder Query => new QueryBuilder(_url);
    }
}