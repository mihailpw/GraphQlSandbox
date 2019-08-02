using System;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Registrar;
using GQL.Services.Infra.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GQL.Services.Infra
{
    public static class GraphQlRegistrarExtensions
    {
        public static IServiceCollection AddGraphQl<TSchema>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TSchema : class
        {
            services.AddSingleton<TSchema>();

            var registry = new GraphQlTypeRegistry();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IGraphQlPartsFactory, GraphQlPartsFactory>();
            services.TryAddSingleton<IGraphQlTypeRegistry>(registry);

            services.TryAddScoped(typeof(AutoEnumerationGraphType<>));
            services.TryAddScoped(typeof(AutoInputObjectGraphType<>));
            services.TryAddScoped(typeof(AutoInterfaceGraphType<>));
            services.TryAddScoped(typeof(AutoInputObjectGraphType<>));

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