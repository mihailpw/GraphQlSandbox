using System;
using GQL.Services.Infra.Core;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra.Registrar
{
    internal class Configurator : IConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly IGraphQlTypeRegistry _typeRegistry;


        public Configurator(IServiceCollection services, IGraphQlTypeRegistry typeRegistry)
        {
            _services = services;
            _typeRegistry = typeRegistry;
        }


        public IConfigurator RegisterObject<T>(Action<IMappingConfigurator> configureAction = null) where T : class
        {
            _services.AddTransient<T>();
            var registeredType = _typeRegistry.RegisterObject(typeof(T));
            if (configureAction != null)
            {
                var configurator = new MappingConfigurator(registeredType, _typeRegistry);
                configureAction(configurator);
            }
            return this;
        }

        public IConfigurator RegisterInterface<T>(Action<IMappingConfigurator> configureAction = null) where T : class
        {
            var registeredType = _typeRegistry.RegisterInterface(typeof(T));
            if (configureAction != null)
            {
                var configurator = new MappingConfigurator(registeredType, _typeRegistry);
                configureAction(configurator);
            }
            return this;
        }

        public IConfigurator RegisterInputObject<T>() where T : class
        {
            _typeRegistry.RegisterInputObject(typeof(T));
            return this;
        }
    }
}