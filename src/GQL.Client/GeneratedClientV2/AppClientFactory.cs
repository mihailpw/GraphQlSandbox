using System;
using GQL.Client.GeneratedClient.Dto;
using GQL.Client.InfraV2;

namespace GQL.Client.GeneratedClientV2
{
    public class AppClientFactory
    {
        private readonly string _url;


        public AppClientFactory(string url)
        {
            _url = url;
        }


        public ClientFactory<QueryDto> ForQuery(Action<QueryType> setupAction)
        {
            var type = new QueryType();
            setupAction(type);
            return new ClientFactory<QueryDto>(_url, "query", type);
        }
    }
}