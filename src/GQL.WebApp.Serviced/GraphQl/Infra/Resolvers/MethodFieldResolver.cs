using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.WebApp.Serviced.GraphQl.Infra.Providers;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Resolvers
{
    public class MethodFieldResolver : IFieldResolver
    {
        private readonly Type _targetType;
        private readonly MethodInfo _methodInfo;
        private readonly IProvider _provider;


        public MethodFieldResolver(Type targetType, MethodInfo methodInfo, IProvider provider)
        {
            _targetType = targetType;
            _methodInfo = methodInfo;
            _provider = provider;
        }


        public object Resolve(ResolveFieldContext context)
        {
            var target = _provider.Get(_targetType);
            var parameters = ProcessParameters(context).ToArray();
            return _methodInfo.Invoke(target, parameters);
        }


        private IEnumerable<object> ProcessParameters(ResolveFieldContext context)
        {
            foreach (var parameterInfo in _methodInfo.GetParameters())
            {
                if (parameterInfo.ParameterType.CheckIfResolveFieldContextType())
                {
                    if (parameterInfo.ParameterType.IsGenericType)
                    {
                        var contextSourceType = parameterInfo.ParameterType.GenericTypeArguments[0];
                        if (parameterInfo.ParameterType.CheckIfResolveFieldContextType(_targetType))
                        {
                            yield return ConvertUtils.ChangeResolveFieldContextTypeTo(context, contextSourceType);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Provided context with {contextSourceType.Name} type can not be processed. Context type can be only with {_targetType.Name} type.");
                        }
                    }
                    else
                    {
                        yield return context;
                    }
                }
                else
                {
                    var value = context.Arguments.GetValueOrDefault(ProviderUtils.GetName(parameterInfo));
                    var convertedValue = ConvertUtils.ChangeTypeTo(value, parameterInfo.ParameterType);
                    yield return convertedValue;
                }
            }
        }
    }
}