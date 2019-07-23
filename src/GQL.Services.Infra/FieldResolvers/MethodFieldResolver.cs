using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.FieldResolvers.Mapping;
using GQL.Services.Infra.Helpers;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GQL.Services.Infra.FieldResolvers
{
    public class MethodFieldResolver : IFieldResolver
    {
        private readonly Type _serviceType;
        private readonly Type _returnType;
        private readonly Type _returnRealType;
        private readonly MethodInfo _methodInfo;
        private readonly IProvider _provider;

        private bool _isInitialized;
        private IObjectMapper _objectMapper;


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

            //if (!_isInitialized)
            //{
            //    var realResultType = _provider.Get(_returnRealType).GetType();
            //    var resultType = result.GetType();
            //    if (realResultType != resultType)
            //    {
            //        _objectMapper = new SingleObjectMapper(realResultType, resultType);
            //        if (realResultType.IsEnumerable())
            //        {
            //            _objectMapper = new ManyObjectMapper(_objectMapper);
            //        }
            //    }

            //    _isInitialized = true;
            //}

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
                if (TypeUtils.ResolveFieldContext.IsInType(parameterInfo.ParameterType))
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
                    var value = context.Arguments.GetValueOrDefault(parameterInfo.GetNameOrDefault(parameterInfo.Name));
                    var convertedValue = ConvertUtils.ChangeTypeTo(value, parameterInfo.ParameterType);
                    yield return convertedValue;
                }
            }
        }
    }
}