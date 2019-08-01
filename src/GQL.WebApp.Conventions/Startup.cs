using GQL.DAL;
using GQL.WebApp.Conventions.Schema;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GQL.WebApp.Conventions
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;


        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(o => o.AllowSynchronousIO = true);

            services.AddScoped<Query>();

            services.AddControllers()
                .AddNewtonsoftJson();
            //services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));
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

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions { GraphQLEndPoint = PathString.FromUriComponent("/graphql") });

            app.UseRouting();
            app.UseEndpoints(e => e.MapControllers());

            //using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            //DbSeeder.Seed(dbContext);
        }
    }
}
