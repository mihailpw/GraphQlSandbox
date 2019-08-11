using System.Reflection;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Extensions.Core
{
    internal class MethodFieldResolver : IFieldResolver
    {
        private readonly object _target;
        private readonly MethodInfo _method;

        private readonly ArgumentsBuilder _argumentsBuilder;


        public MethodFieldResolver(object target, MethodInfo method)
        {
            _target = target;
            _method = method;

            _argumentsBuilder = new ArgumentsBuilder(method);
        }


        public object Resolve(ResolveFieldContext context)
        {
            var arguments = _argumentsBuilder.Build(context);

            return _method.Invoke(_target, arguments);
        }
    }
}