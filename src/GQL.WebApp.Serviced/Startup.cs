using System;
using GQL.DAL;
using GQL.WebApp.Serviced.GraphQlV2;
using GQL.WebApp.Serviced.Infra;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GQL.WebApp.Serviced
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;


        public Startup(IHostingEnvironment environment)
        {
            _environment = environment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var isDev = _environment.IsDevelopment();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(typeof(IProvider), typeof(Provider));
            services.AddTransient(typeof(IProvider<>), typeof(Provider<>));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();

            services.AddTransient<QueryRootService>();
            services.AddTransient<IInnerService, InnerService>();

            services.AddSingleton<GraphQlSchema>();

            services
                .AddGraphQL(o =>
                {
                    o.EnableMetrics = isDev;
                    o.ExposeExceptions = isDev;
                })
                .AddWebSockets();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider provider)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseWebSockets();

            app.UseGraphQL<GraphQlSchema>();
            app.UseGraphQLWebSockets<GraphQlSchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { GraphQLEndPoint = PathString.FromUriComponent("/graphql") });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                DbSeeder.Seed(dbContext);
            }

            ProviderContext.Instance = provider.GetService<IProvider>();
        }
    }
}
