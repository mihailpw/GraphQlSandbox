using System;
using System.Text;
using System.Threading.Tasks;
using GQL.Client.GeneratedClient;
using GQL.Client.GeneratedClient.Dto;
using GraphQlClientGenerator;

namespace GQL.Client
{
    internal class Program
    {
        private const string Url = "https://localhost:5001/graphql";


        public static async Task Main(string[] args)
        {
            await GenerateClient();

            var clientProvider = new AppClientProvider(Url);

            var query = clientProvider
                .Query(q => q
                    .User("e6714b76-74da-4931-8f61-09b690a327c4").With(u => u
                        .Id()
                        .Email())
                    .Users().With(u => u
                        .Id()
                        .Email()
                        .Friends().With(f => f
                            .Id()
                            .Name()
                            .Email())));

            var queryResponse = await query.RequestAsync();

            var mutation = new AppClientProvider(Url)
                .Mutation(q => q
                    .CreateUser(
                        new UserInputDto { Email = "ss@ss.ss", Name = "ss s ss" }).With(u => u
                        .Id()
                        .Email()));

            var mutationResponse = await mutation.RequestAsync();

            queryResponse = await query.RequestAsync();

            Console.ReadLine();
        }

        private static async Task GenerateClient()
        {
            var schema = await GraphQlGenerator.RetrieveSchema(Url);

            var builder = new StringBuilder();
            GraphQlGenerator.GenerateQueryBuilder(schema, builder);
            GraphQlGenerator.GenerateDataClasses(schema, builder);

            var generatedClasses = builder.ToString();

            ;
        }
    }
}
