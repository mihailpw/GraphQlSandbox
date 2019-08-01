using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GQL.WebApp.Annotated
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(ConfigureLogging)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }


        private static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddConsole(c => c.IncludeScopes = true);
        }
    }
}
