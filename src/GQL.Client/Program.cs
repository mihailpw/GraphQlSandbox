using System;
using System.Threading.Tasks;
using GQL.Client.QueryBuilders;

namespace GQL.Client
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var requestBuilder = new AppClient("https://localhost:5001/graphql")
                .Query
                .IncludeUser(b => b
                    // .FilterId("939439dc-5608-4eee-9578-1ba6a6f64d48")
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
