using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQlV2._
{
    internal class MethodFieldResolver : IFieldResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly Type _outType;
        private readonly IProvider _provider;
        private readonly bool _isEnumerable;


        public MethodFieldResolver(MethodInfo methodInfo, Type outType, IProvider provider)
        {
            _methodInfo = methodInfo;
            _outType = outType;
            _provider = provider;
            _isEnumerable = TypeUtils.Enumerable.IsInType(_methodInfo.ReturnType);
        }


        public object Resolve(ResolveFieldContext context)
        {
            var arguments = ReflectionHelper.BuildArguments(_methodInfo.GetParameters(), context);
            var target = _outType.IsInstanceOfType(context.Source)
                ? context.Source
                : _provider.Get(_methodInfo.DeclaringType);

            if (target == null)
                throw new InvalidOperationException($"Could not resolve an instance of {_methodInfo.DeclaringType.Name} to execute {(context.ParentType != null ? $"{context.ParentType.Name}." : null)}{context.FieldName}");

            var methodExecutionResult = _methodInfo.Invoke(target, arguments);

            if (_methodInfo.ReturnType == _outType)
            {
                return methodExecutionResult;
            }

            if (_isEnumerable)
            {
                var methodExecutionResults = (IEnumerable<object>) methodExecutionResult;
                var returnValues = methodExecutionResults.Select(GetServiceAndPopulate);

                return returnValues;
            }

            return GetServiceAndPopulate(methodExecutionResult);
        }


        private object GetServiceAndPopulate(object value)
        {
            var returnValue = _provider.Get(_outType);
            ObjectMapper.Instance.Populate(value, returnValue);

            return returnValue;
        }
    }
}