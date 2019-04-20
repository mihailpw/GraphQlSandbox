using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GQL.WebApp.Typed.GraphQl.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGraphSchema<T>(this IServiceCollection services) where T : class, ISchema
        {
            Add<ISchema, T>(services);
            Add<T, T>(services);
        }

        public static void AddGraphQuery<T>(this IServiceCollection services) where T : GraphQuery
        {
            Add<GraphQuery, T>(services);
        }

        public static void AddGraphMutation<T>(this IServiceCollection services) where T : GraphMutation
        {
            Add<GraphMutation, T>(services);
        }

        public static void AddGraphSubscription<T>(this IServiceCollection services) where T : GraphSubscription
        {
            Add<GraphSubscription, T>(services);
        }

        private static void Add<TService, TImplementation>(IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.TryAddScoped<TService, TImplementation>();
        }
    }
}