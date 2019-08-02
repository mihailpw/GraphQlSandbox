using System;
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
            ServiceProvider = serviceProvider;
            TypeRegistry = serviceProvider.GetService<IGraphQlTypeRegistry>();
            PartsFactory = serviceProvider.GetService<IGraphQlPartsFactory>();
            Config = new Config();
        }
    }
}