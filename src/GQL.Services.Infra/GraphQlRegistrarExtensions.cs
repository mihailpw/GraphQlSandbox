using System;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Registrar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra
{
    public static class GraphQlRegistrarExtensions
    {
        public static IServiceCollection AddGraphQl<TSchema>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TSchema : class
        {
            services.AddSingleton<TSchema>();

            var registry = new GraphQlTypeRegistry();
            services.AddTransient<IScopedProvider, ScopedScopedProvider>();
            services.AddSingleton<IGraphQlPartsFactory, GraphQlPartsFactory>();
            services.AddSingleton<IGraphQlTypeRegistry>(registry);

            var options = new Configurator(services, registry);
            configureAction(options);

            return services;
        }

        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            GlobalContext.Populate(builder.ApplicationServices);
            return builder;
        }
    }
}