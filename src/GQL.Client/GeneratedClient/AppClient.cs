using System;
using System.Linq;
using System.Text;
using GQL.Client.Dto;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public class AppClient
    {
        private readonly string _url;
        private readonly bool _useGetRequest;


        public AppClient(string url, bool useGetRequest = true)
        {
            _url = url;
            _useGetRequest = useGetRequest;
        }


        public IClient<QueryDto> Query(Action<IQueryType> action)
        {
            var type = new QueryType();
            action(type);
            var rootType = new RootType("query", type);

            var stringBuilder = new StringBuilder();
            rootType.AppendQuery(stringBuilder);
            var query = stringBuilder.ToString();

            var variables = rootType.GetArguments().ToDictionary(a => a.ArgumentName, a => a.Value);

            return new Client<QueryDto>(_url, query, variables, _useGetRequest);
        }
    }
}