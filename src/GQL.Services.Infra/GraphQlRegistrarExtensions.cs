using System;
using GQL.Services.Infra.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra
{
    public static class GraphQlRegistrarExtensions
    {
        public class Options
        {
            private readonly IServiceCollection _services;


            public Options(IServiceCollection services)
            {
                _services = services;
            }


            public Options RegisterObject<TImplementation>() where TImplementation : class
            {
                _services.AddTransient<TImplementation>();
                GraphQlTypeRegistry.Instance.RegisterObject(typeof(TImplementation));
                return this;
            }

            public Options RegisterObject<TService, TImplementation>() where TService : class where TImplementation : class, TService
            {
                _services.AddTransient<TService, TImplementation>();
                GraphQlTypeRegistry.Instance.RegisterObject(typeof(TService));
                return this;
            }
        }


        public static IServiceCollection AddGraphQl<TSchema>(this IServiceCollection services, Action<Options> configureAction)
            where TSchema : class
        {
            services.AddSingleton<TSchema>();

            services.AddTransient<IProvider, ScopedProvider>();

            var options = new Options(services);
            configureAction(options);

            return services;
        }

        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            ProviderContext.Instance = builder.ApplicationServices.GetService<IProvider>();

            return builder;
        }
    }
}