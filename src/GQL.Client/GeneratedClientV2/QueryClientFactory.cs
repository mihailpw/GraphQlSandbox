using GQL.Client.GeneratedClient.Dto;
using GQL.Client.InfraV2;

namespace GQL.Client.GeneratedClientV2
{
    public class QueryClientFactory : ClientFactory<QueryDto>
    {
        public QueryClientFactory(string url, QueryType type)
            : base(url, "query", type)
        {
        }
    }
}