using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GQL.WebApp.Serviced.GraphQlV2._
{
    public class ObjectMapper
    {
        private class CachedObjectMapper
        {
            private const BindingFlags PropertiesFlags = BindingFlags.Instance | BindingFlags.Public;

            private readonly Dictionary<string, PropertyInfo> _targetProperties;
            private readonly List<PropertyInfo> _sourceProperties;


            public CachedObjectMapper(IReflect source, IReflect target)
            {
                _targetProperties = target.GetProperties(PropertiesFlags).Where(p => p.CanWrite).ToDictionary(p => p.Name);
                _sourceProperties = source.GetProperties(PropertiesFlags).Where(p => p.CanRead && _targetProperties.ContainsKey(p.Name)).ToList();
            }


            public void Populate(object source, object target)
            {
                foreach (var sourceProperty in _sourceProperties)
                {
                    var value = sourceProperty.GetValue(source);
                    var targetProperty = _targetProperties[sourceProperty.Name];
                    targetProperty.SetValue(target, value);
                }
            }
        }

        public static readonly ObjectMapper Instance = new ObjectMapper();

        private readonly Dictionary<(Type source, Type target), CachedObjectMapper> _cachedObjectMappers;


        public ObjectMapper()
        {
            _cachedObjectMappers = new Dictionary<(Type, Type), CachedObjectMapper>();
        }


        public object Populate(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();
            var key = (sourceType, targetType);

            if (!_cachedObjectMappers.TryGetValue(key, out var cachedObjectMapper))
            {
                cachedObjectMapper = new CachedObjectMapper(sourceType, targetType);
                _cachedObjectMappers.Add(key, cachedObjectMapper);
            }

            cachedObjectMapper.Populate(source, target);

            return targetType;
        }
    }
}