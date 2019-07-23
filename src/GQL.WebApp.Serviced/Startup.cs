using System;
using GQL.DAL;
using GQL.Services.Infra;
using GQL.Services.Infra.Core;
using GQL.WebApp.Serviced.GraphQlV2;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddHttpContextAccessor();

            services.AddGraphQl<GraphQlSchema>(c => c
                .RegisterObject<QueryRootService>()
                .RegisterObject<IInnerService, InnerService>());

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));

            services
                .AddGraphQL(o => o.EnableMetrics = o.ExposeExceptions = isDev)
                .AddWebSockets();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphQl();

            app.UseWebSockets();

            app.UseGraphQL<GraphQlSchema>();
            app.UseGraphQLWebSockets<GraphQlSchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { GraphQLEndPoint = PathString.FromUriComponent("/graphql") });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                DbSeeder.Seed(dbContext);
            }
        }
    }
}
