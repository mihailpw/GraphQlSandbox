using System;
using System.Reflection;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.FieldResolvers
{
    internal class PropertyFieldResolver : IFieldResolver
    {
        private readonly PropertyInfo _serviceProperty;
        private readonly IConfig _config;


        public PropertyFieldResolver(PropertyInfo serviceProperty, IConfig config)
        {
            _serviceProperty = serviceProperty;
            _config = config;
        }


        public object Resolve(ResolveFieldContext context)
        {
            // ReSharper disable once PossibleNullReferenceException
            if (_serviceProperty.DeclaringType.IsInstanceOfType(context.Source))
            {
                return _serviceProperty.GetValue(context.Source);
            }
            else
            {
                var sourceType = context.Source.GetType();
                var propertyInfo = sourceType.GetProperty(_serviceProperty.Name);

                if (propertyInfo == null)
                {
                    if (_config.ThrowIfPropertyNotFound)
                    {
                        throw new InvalidOperationException($"Property {_serviceProperty.Name} not found in {sourceType.Name} type.");
                    }

                    return ActivatorHelper.CreateDefault(_serviceProperty.PropertyType);
                }

                if (_config.ThrowIfPropertiesTypesDifferent)
                {
                    if (!_serviceProperty.PropertyType.IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        throw new InvalidOperationException($"Property {_serviceProperty.Name} in {sourceType.Name} type does not match service's return ({propertyInfo.PropertyType.Name} instead of expected {_serviceProperty.PropertyType.Name}).");
                    }
                }

                return propertyInfo.GetValue(context.Source);
            }
        }
    }
}