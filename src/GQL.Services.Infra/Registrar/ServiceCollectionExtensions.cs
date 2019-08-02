using System;
using GQL.Services.Infra.Common;
using GQL.Services.Infra.Common.Schemas;
using GQL.Services.Infra.Common.Types;
using GQL.Services.Infra.Registrar.Configurators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GQL.Services.Infra.Registrar
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGraphQlQuery<TQuery>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TQuery : class
        {
            services.AddSingleton<QuerySchema<TQuery>>();

            var configurator = Register(services);
            configurator.RegisterObject<TQuery>();
            configureAction(configurator);

            return services;
        }

        public static IServiceCollection AddGraphQlQueryMutation<TQuery, TMutation>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TQuery : class
            where TMutation : class
        {
            services.AddSingleton<QueryMutationSchema<TQuery, TMutation>>();

            var configurator = Register(services);
            configurator.RegisterObject<TQuery>();
            configurator.RegisterObject<TMutation>();
            configureAction(configurator);

            return services;
        }

        public static IServiceCollection AddGraphQlQuerySubscription<TQuery, TSubscription>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TQuery : class
            where TSubscription : class
        {
            services.AddSingleton<QueryMutationSchema<TQuery, TSubscription>>();

            var configurator = Register(services);
            configurator.RegisterObject<TQuery>();
            configurator.RegisterObject<TSubscription>();
            configureAction(configurator);

            return services;
        }

        public static IServiceCollection AddGraphQlQueryMutationSubscription<TQuery, TMutation, TSubscription>(this IServiceCollection services, Action<IConfigurator> configureAction)
            where TQuery : class
            where TMutation : class
            where TSubscription : class
        {
            services.AddSingleton<QueryMutationSubscriptionSchema<TQuery, TMutation, TSubscription>>();

            var configurator = Register(services);
            configurator.RegisterObject<TQuery>();
            configurator.RegisterObject<TMutation>();
            configurator.RegisterObject<TSubscription>();
            configureAction(configurator);

            return services;
        }


        private static IConfigurator Register(IServiceCollection services)
        {
            var typeRegistry = new GraphQlTypeRegistry();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IGraphQlPartsFactory, GraphQlPartsFactory>();
            services.TryAddSingleton<IGraphQlTypeRegistry>(typeRegistry);

            services.TryAddSingleton(typeof(AutoEnumerationGraphType<>));
            services.TryAddSingleton(typeof(AutoInputObjectGraphType<>));
            services.TryAddSingleton(typeof(AutoInterfaceGraphType<>));
            services.TryAddSingleton(typeof(AutoInputObjectGraphType<>));

            return new Configurator(services, typeRegistry);
        }
    }
}