using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GQL.Client.GeneratedClientV2;

namespace GQL.Client
{
    internal class Program
    {
        private const string Url = "https://localhost:5001/graphql";

        public static async Task Main(string[] args)
        {
            var clientFactory = new AppClientFactory(Url)
                .ForQuery(q => q
                    .User("2708fef5-5ff9-442b-b3d6-113757a7fba8")(u => u
                        .Friends()(f => f
                            .Id()
                            .OnCustomerType(c => c
                                .IsActive()
                            )
                        )
                        .OnCustomerType(c => c
                            .IsActive()
                        )
                    )
                    .Users()(u => u
                        .Id()
                        .Name()
                        .Friends()(f => f
                            .Id()
                            .OnCustomerType(c => c
                                .IsActive()
                            )
                        )
                        .OnCustomerType(c => c
                            .IsActive()
                        )
                    )
                );

            var r = await clientFactory.CreateClient().RequestAsync();

            await GenerateClient();

            //var builder = new UsersQueryQueryBuilder()
            //    .WithUser(
            //        new UserInterfaceQueryBuilder()
            //            .WithEmail()
            //            .WithFriends(
            //                new UserInterfaceQueryBuilder()
            //                    .WithAllScalarFields(),
            //                email: "ss@ss.ss"),
            //        Guid.NewGuid())
            //    .WithUsers(
            //        new UserInterfaceQueryBuilder()
            //            .WithEmail()
            //            .WithId());

            //var r = builder.Build(Formatting.Indented);

            //var clientProvider = new AppClientProvider(Url);

            //var query = clientProvider
            //    .Query(q => q
            //        .UserF()(u => u
            //            .Id()
            //            .Email()
            //        )
            //        .Users().With(u => u
            //            .Id()
            //            .Email()
            //            .Friends().With(f => f
            //                .Id()
            //                .Name()
            //                .Email()
            //            )
            //        )
            //    );

            //var queryResponse = await query.RequestAsync();

            //var mutation = new AppClientProvider(Url)
            //    .Mutation(q => q
            //        .CreateUser(
            //            new UserInputDto { Email = "ss@ss.ss", Name = "ss s ss" }).With(u => u
            //            .Id()
            //            .Email()));

            //var mutationResponse = await mutation.RequestAsync();

            //queryResponse = await query.RequestAsync();

            Console.ReadLine();
        }

        private static async Task GenerateClient()
        {
            //var schema = await GraphQlGenerator.RetrieveSchema(Url);

            //var builder = new StringBuilder();
            //GraphQlGenerator.GenerateQueryBuilder(schema, builder);
            //GraphQlGenerator.GenerateDataClasses(schema, builder);

            //var generatedClasses = builder.ToString();

            ;
        }
    }
}
