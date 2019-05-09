using System;
using System.Threading.Tasks;
using GraphQlClient;
using Newtonsoft.Json;

namespace GQL.Client
{
    internal class Program
    {
        private const string Url = "https://localhost:5001/graphql";

        public static async Task Main(string[] args)
        {
            var clientFactory = new AppClientFactory(Url);

            var client = clientFactory
                .CreateQueryClient(q => q
                    .User("f8dc429d-17e2-4917-89d6-227127376bc0")(u => u
                        .Friends()(f => f
                            .Id()
                            .OnCustomer(c => c
                                .IsActive()))
                        .OnCustomer(c => c
                            .IsActive()))
                    .Users()(u => u
                        .Id()
                        .Name()
                        .Friends()(f => f
                            .Id()
                            .OnCustomer(c => c
                                .IsActive()))
                        .OnCustomer(c => c
                            .IsActive())
                        .OnManager(m => m
                            .NumberOfSales()))
                    .Customers()(c => c
                        .Name()
                        .IsActive()));

            var response = await client.SendAsync();

            var subscription = await clientFactory
                .CreateSubscriptionClient(s => s
                    .AddUser()(u => u
                        .Id()
                        .Name()
                        .Email()))
                .SendAsync();

            subscription.Received += (sender, r) => Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

            await Task.Delay(1000);

            var mutationResponse = await clientFactory
                .CreateMutationClient(m => m
                    .CreateManager(new ManagerUserInputDto { Name = "name", Email = "email@mail.m" })(u => u
                        .Id()
                        .Name()
                        .Email()))
                .SendAsync();

            await Task.Delay(1000);

            for (int i = 0; i < 100; i++)
            {
                var i1 = i;
                await clientFactory
                    .CreateMutationClient(m => m
                        .CreateManager(new ManagerUserInputDto { Name = $"name_{i1:D3}", Email = $"email_{i1:D3}@mail.m" })(u => u
                            .Id()
                            .Name()
                            .Email()))
                    .SendAsync();
            }


            Console.ReadKey();
        }
    }
}
