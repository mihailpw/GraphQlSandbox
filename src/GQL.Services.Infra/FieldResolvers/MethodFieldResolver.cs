using System;
using System.Linq;
using System.Reflection;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.FieldResolvers
{
    internal class MethodFieldResolver : IFieldResolver
    {
        private readonly Type _serviceType;
        private readonly MethodInfo _serviceMethod;
        private readonly IServiceProvider _serviceProvider;


        public MethodFieldResolver(Type serviceType, MethodInfo serviceMethod, IServiceProvider serviceProvider)
        {
            _serviceType = serviceType;
            _serviceMethod = serviceMethod;
            _serviceProvider = serviceProvider;
        }


        public object Resolve(ResolveFieldContext context)
        {
            var arguments = GraphQlUtils.BuildArguments(_serviceMethod, context).ToArray();

            var target = _serviceType.IsInstanceOfType(context.Source)
                ? context.Source
                : _serviceProvider.GetService(_serviceType);

            if (target == null)
                throw new InvalidOperationException($"Could not resolve an instance of {_serviceType.Name} to execute {(context.ParentType != null ? $"{context.ParentType.Name}." : null)}{context.FieldName}");

            return _serviceMethod.Invoke(target, arguments);
        }
    }
}