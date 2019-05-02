using System;
using System.Threading.Tasks;
using GQL.Client.GeneratedClient;

namespace GQL.Client
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new AppClient("https://localhost:5001/graphql")
                .Query(q => q
                    .User("e6714b76-74da-4931-8f61-09b690a327c4", u => u
                        .Id()
                        .Email())
                    .Users(u => u
                        .Id()
                        .Email()
                        .Friends(null, f => f
                            .Id()
                            .Name()
                            .Email())));

            var response = await client.RequestAsync();


            Console.ReadLine();
        }
    }
}
