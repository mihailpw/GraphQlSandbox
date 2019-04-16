using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Typed.GraphQl.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGraphQuery<T>(this IServiceCollection services) where T : GraphQuery
        {
            services.AddScoped<GraphQuery, T>();
        }

        public static void AddGraphMutation<T>(this IServiceCollection services) where T : GraphMutation
        {
            services.AddScoped<GraphMutation, T>();
        }
    }
}