using GQL.Services.Infra.Common;
using GQL.Services.Infra.Common.Schemas;
using Microsoft.AspNetCore.Builder;
using GraphQL.Server;
using GraphQL.Types;

namespace GQL.Services.Infra.Registrar
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGraphQlQuery<TQuery>(this IApplicationBuilder builder, string path = "/graphql")
        {
            return Configure<QuerySchema<TQuery>>(builder, path, false);
        }

        public static IApplicationBuilder UseGraphQlQueryMutation<TQuery, TMutation>(this IApplicationBuilder builder, string path = "/graphql")
        {
            return Configure<QueryMutationSchema<TQuery, TMutation>>(builder, path, false);
        }

        public static IApplicationBuilder UseGraphQlQuerySubscription<TQuery, TSubscription>(this IApplicationBuilder builder, string path = "/graphql")
        {
            return Configure<QuerySubscriptionSchema<TQuery, TSubscription>>(builder, path, true);
        }

        public static IApplicationBuilder UseGraphQlQueryMutationSubscription<TQuery, TMutation, TSubscription>(this IApplicationBuilder builder, string path = "/graphql")
        {
            return Configure<QueryMutationSubscriptionSchema<TQuery, TMutation, TSubscription>>(builder, path, true);
        }


        private static IApplicationBuilder Configure<TSchema>(IApplicationBuilder builder, string path, bool withWebSockets)
            where TSchema : ISchema
        {
            GlobalContext.Populate(builder.ApplicationServices);

            builder.UseGraphQL<TSchema>(path);
            if (withWebSockets)
            {
                builder.UseGraphQLWebSockets<TSchema>(path);
            }

            return builder;
        }
    }
}