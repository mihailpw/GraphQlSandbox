using System;
using System.Threading.Tasks;
using GQL.Client.QueryBuilders;
using GraphQL.Client;

namespace GQL.Client
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new GraphQLClient("https://localhost:5001/graphql");

            var requestBuilder = new AppClient("https://localhost:5001/graphql")
                .IncludeUser(b => b
                    .FilterId("bc7b2135-e5a5-4034-aec8-96cf8d551671")
                    .IncludeId()
                    .IncludeEmail())
                .IncludeUsers(b => b
                    .IncludeId()
                    .IncludeEmail());

            var response = await requestBuilder.SendAsync();


            Console.ReadLine();
        }
    }
}
