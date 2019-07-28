using System;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra.Core
{
    internal static class GlobalContext
    {
        public static IGraphQlTypeRegistry TypeRegistry { get; private set; }
        public static IGraphQlPartsFactory PartsFactory { get; private set; }
        public static IScopedProvider ScopedProvider { get; private set; }
        public static IConfig Config { get; private set; }


        public static void Populate(IServiceProvider serviceProvider)
        {
            TypeRegistry = serviceProvider.GetService<IGraphQlTypeRegistry>();
            PartsFactory = serviceProvider.GetService<IGraphQlPartsFactory>();
            ScopedProvider = serviceProvider.GetService<IScopedProvider>();
            Config = new Config();
        }
    }
}