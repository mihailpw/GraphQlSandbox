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
                .IncludeUser(
                    b => b.IncludeId()
                        .IncludeEmail())
                .IncludeUsers(
                    b => b.IncludeId()
                        .IncludeEmail());

            var response = await requestBuilder.SendAsync();


            Console.ReadLine();
        }
    }
}
