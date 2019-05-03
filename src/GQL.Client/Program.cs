using System;
using System.Threading.Tasks;
using GQL.Client.Dto;
using GQL.Client.GeneratedClient;

namespace GQL.Client
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var clientProvider = new AppClientProvider("https://localhost:5001/graphql");

            var query = clientProvider
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

            var queryResponse = await query.RequestAsync();

            var mutation = new AppClientProvider("https://localhost:5001/graphql")
                .Mutation(q => q
                    .CreateUser(new UserInputDto { Email = "ss@ss.ss", Name = "ss s ss" }, u => u
                        .Id()
                        .Email()));

            var mutationResponse = await mutation.RequestAsync();

            queryResponse = await query.RequestAsync();

            Console.ReadLine();
        }
    }
}
