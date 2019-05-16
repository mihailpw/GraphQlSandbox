using System;
using System.Linq;
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
                    .Users(UserType.BadGuy)(u => u
                        .Name()
                        .Type()));

            var response = await client.SendAsync();

            var subscription = await clientFactory
                .CreateSubscriptionClient(s => s
                    .AddUser()(u => u
                        .Id()
                        .OnManager(m => m
                            .NumberOfSales())
                        .OnCustomer(c => c
                            .IsActive())))
                .SendAsync();

            subscription.Received += (sender, r) => Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));

            await Task.Delay(1000);

            var customers = Enumerable
                .Range(1, 2)
                .Select(i => new CustomerInputDto { Name = $"name_{i:D3}", Email = $"email_{i:D3}@mail.m" })
                .ToList();

            var mutationResponse = await clientFactory
                .CreateMutationClient(m => m
                    .CreateManager(new ManagerInputDto { Name = "name", Email = "email@mail.m" })(u => u
                        .Id()
                        .Name()
                        .Email())
                    .CreateCustomers(customers)(c => c
                        .Id()))
                .SendAsync();

            await Task.Delay(1000);

            subscription.Dispose();

            var mutationResponse2 = await clientFactory
                .CreateMutationClient(m => m
                    .CreateManager(new ManagerInputDto { Name = "name", Email = "email@mail.m" })(u => u
                        .Id()
                        .Name()
                        .Email())
                    .CreateCustomers(customers)(c => c
                        .Id()))
                .SendAsync();

            Console.ReadKey();

        }
    }
}
