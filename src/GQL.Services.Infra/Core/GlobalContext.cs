using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra.Core
{
    internal static class GlobalContext
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IGraphQlTypeRegistry TypeRegistry { get; private set; }
        public static IGraphQlPartsFactory PartsFactory { get; private set; }
        public static IConfig Config { get; private set; }


        public static void Populate(IServiceProvider serviceProvider)
        {
            ServiceProvider = new RequestServicesProvider(serviceProvider.GetService<IHttpContextAccessor>());
            TypeRegistry = serviceProvider.GetService<IGraphQlTypeRegistry>();
            PartsFactory = serviceProvider.GetService<IGraphQlPartsFactory>();
            Config = new Config();
        }
    }
}