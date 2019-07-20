using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.WebApp.Serviced.GraphQl.Infra.Providers;
using GQL.WebApp.Serviced.GraphQlV2.Infra;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Resolvers
{
    public class MethodFieldResolver : IFieldResolver
    {
        private readonly Type _serviceType;
        private readonly Type _returnType;
        private readonly Type _returnRealType;
        private readonly MethodInfo _methodInfo;
        private readonly IProvider _provider;

        private bool _isFirstCall = true;
        private ObjectMapper _objectMapper;


        public MethodFieldResolver(Type serviceType, Type returnType, Type returnRealType, MethodInfo methodInfo, IProvider provider)
        {
            _serviceType = serviceType;
            _returnType = returnType;
            _returnRealType = returnRealType;
            _methodInfo = methodInfo;
            _provider = provider;
        }


        public object Resolve(ResolveFieldContext context)
        {
            var service = context.Source ?? _provider.Get(_serviceType);
            var parameters = ProcessParameters(context).ToArray();
            var result = _methodInfo.Invoke(service, parameters);

            if (_isFirstCall)
            {
                var realResultType = _provider.Get(_returnRealType).GetType();
                var resultType = result.GetType();
                if (realResultType != resultType)
                {
                    _objectMapper = new ObjectMapper(realResultType, resultType);
                }
            }

            if (_objectMapper == null)
            {
                return result;
            }
            else
            {
                var realResult = _provider.Get(_returnRealType);
                _objectMapper.Populate(realResult, result);
                return realResult;
            }
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
                        if (contextSourceType == _returnType)
                        {
                            yield return ConvertUtils.ChangeResolveFieldContextTypeTo(context, contextSourceType);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Provided context with {contextSourceType.Name} type can not be processed. Context type can be only with {_returnType.Name} type.");
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