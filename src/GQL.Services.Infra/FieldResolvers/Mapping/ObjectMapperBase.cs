using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GQL.Services.Infra.FieldResolvers.Mapping
{
    public abstract class ObjectMapperBase : IObjectMapper
    {
        private const BindingFlags PropertiesFlags = BindingFlags.Instance | BindingFlags.Public;

        private readonly Dictionary<string, PropertyInfo> _targetPropertiesDictionary;
        private readonly IEnumerable<PropertyInfo> _sourceProperties;


        protected ObjectMapperBase(IReflect targetType, IReflect sourceType)
        {
            _targetPropertiesDictionary = targetType.GetProperties(PropertiesFlags).Where(pi => pi.CanWrite).ToDictionary(p => p.Name, p => p);
            _sourceProperties = sourceType.GetProperties(PropertiesFlags).Where(pi => pi.CanRead && _targetPropertiesDictionary.ContainsKey(pi.Name));
        }


        public abstract object Populate(object targetType, object sourceType);


        protected object PopulateInternal(object targetType, object sourceType)
        {
            foreach (var sourceProperty in _sourceProperties)
            {
                var value = sourceProperty.GetValue(sourceType);
                var targetProperty = _targetPropertiesDictionary[sourceProperty.Name];
                targetProperty.SetValue(targetType, value);
            }

            return targetType;
        }
    }
}