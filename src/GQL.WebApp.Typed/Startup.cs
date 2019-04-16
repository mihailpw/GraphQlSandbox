using GQL.DAL;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Schemas;
using GQL.WebApp.Typed.GraphQl.Schemas.Users;
using GraphiQl;
using GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Typed
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AppDbContext>(b => b.UseInMemoryDatabase(nameof(AppDbContext)));

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();

            services.AddScoped<AppSchema>();
            services.AddGraphQuery<UsersQuery>();
            services.AddGraphMutation<UsersMutation>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphiQl();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                DbSeeder.Seed(dbContext);
            }
        }
    }
}
