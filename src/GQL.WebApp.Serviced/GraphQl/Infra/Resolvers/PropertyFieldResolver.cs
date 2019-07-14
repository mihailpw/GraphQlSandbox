using System;
using System.Reflection;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Resolvers
{
    public class PropertyFieldResolver : IFieldResolver
    {
        private readonly Type _type;
        private readonly PropertyInfo _propertyInfo;
        private readonly IProvider _provider;


        public PropertyFieldResolver(Type type, PropertyInfo propertyInfo, IProvider provider)
        {
            _type = type;
            _propertyInfo = propertyInfo;
            _provider = provider;
        }


        public object Resolve(ResolveFieldContext context)
        {
            return _propertyInfo.GetMethod.Invoke(_provider.Get(_type), new object[0]);
        }
    }
}