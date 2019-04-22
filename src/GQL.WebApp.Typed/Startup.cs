using GQL.DAL;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Schemas;
using GQL.WebApp.Typed.GraphQl.Schemas.Users;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Typed
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();

            services.AddGraphSchema<UsersSchema, UsersQuery, UsersMutation, UsersSubscription>();
            services
                .AddGraphQL(o =>
                {
                    o.EnableMetrics = isDev;
                    o.ExposeExceptions = isDev;
                })
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

            app.UseGraphQL<UsersSchema>();
            app.UseGraphQLWebSockets<UsersSchema>();

            app.UseMvc();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { GraphQLEndPoint = PathString.FromUriComponent("/graphql") });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                DbSeeder.Seed(dbContext);
            }
        }
    }
}
