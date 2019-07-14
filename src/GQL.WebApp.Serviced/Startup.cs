using GQL.DAL;
using GQL.WebApp.Serviced.GraphQl.Schemas.Users;
using GQL.WebApp.Serviced.Infra;
using GQL.WebApp.Serviced.Managers;
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

            services.AddSingleton<IUsersObservable, UsersObservable>();
            services.AddScoped<IUsersManager, UsersManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();

            services.AddSingleton<UsersSchema>();
            services.AddSingleton<UsersQuery>();
            services.AddSingleton<UsersMutation>();
            services.AddSingleton<UsersSubscription>();

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

            app.UseWebSockets();

            app.UseGraphQL<UsersSchema>();
            app.UseGraphQLWebSockets<UsersSchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { GraphQLEndPoint = PathString.FromUriComponent("/graphql") });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                DbSeeder.Seed(dbContext);
            }
        }
    }
}
