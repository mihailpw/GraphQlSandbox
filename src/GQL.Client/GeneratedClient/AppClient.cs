using System;
using GQL.Client.GeneratedClient.Dto;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public class AppClientProvider : ClientProviderBase
    {
        public AppClientProvider(string url, bool usePostRequest = true)
            : base(url, usePostRequest)
        {
        }


        public IClient<QueryDto> Query(Action<IQueryType> action)
        {
            var type = new QueryType();
            action(type);
            return CreateClient<QueryDto>("query", type);
        }

        public IClient<MutationDto> Mutation(Action<IMutationType> action)
        {
            var type = new MutationType();
            action(type);
            return CreateClient<MutationDto>("mutation", type);
        }
    }
}