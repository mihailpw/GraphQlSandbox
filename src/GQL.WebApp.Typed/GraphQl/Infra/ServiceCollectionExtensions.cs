using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Typed.GraphQl.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGraphSchema<TSchema, TQuery>(this IServiceCollection services)
            where TSchema : GraphSchema
            where TQuery : GraphQuery
        {
            services.AddSingleton<TSchema>();
            services.AddSingleton<TQuery>();
        }

        public static void AddGraphSchema<TSchema, TQuery, TMutation>(this IServiceCollection services)
            where TSchema : GraphSchema
            where TQuery : GraphQuery
            where TMutation : GraphMutation
        {
            services.AddGraphSchema<TSchema, TQuery>();
            services.AddSingleton<TMutation>();
        }

        public static void AddGraphSchema<TSchema, TQuery, TMutation, TSubscription>(this IServiceCollection services)
            where TSchema : GraphSchema
            where TQuery : GraphQuery
            where TMutation : GraphMutation
            where TSubscription : GraphSubscription
        {
            services.AddGraphSchema<TSchema, TQuery, TMutation>();
            services.AddSingleton<TSubscription>();
        }
    }
}