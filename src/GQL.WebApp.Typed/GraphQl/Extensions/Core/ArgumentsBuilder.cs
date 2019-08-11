using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Extensions.Core
{
    public class ArgumentsBuilder
    {
        private readonly List<ArgumentMetadata> _argumentsMetadata;


        public ArgumentsBuilder(MethodBase methodInfo)
        {
            _argumentsMetadata = methodInfo.GetParameters().Select(CreateMetadata).ToList();
        }


        public object[] Build(ResolveFieldContext fieldContext)
        {
            return BuildArguments(fieldContext).ToArray(_argumentsMetadata.Count);
        }


        private static ArgumentMetadata CreateMetadata(ParameterInfo parameterInfo)
        {
            var queryArgumentAttribute = parameterInfo.GetCustomAttribute<QueryArgumentAttribute>();

            return new ArgumentMetadata(
                queryArgumentAttribute != null,
                queryArgumentAttribute?.Name ?? parameterInfo.Name,
                parameterInfo.ParameterType);
        }

        private IEnumerable<object> BuildArguments(ResolveFieldContext context)
        {
            foreach (var metadata in _argumentsMetadata)
            {
                if (metadata.IsGraphQlMember)
                {
                    var value = context.GetArgument(metadata.ReturnType, metadata.Name);
                    yield return value;
                }
                else
                {
                    if (metadata.ReturnType.IsInstanceOfType(context.UserContext))
                    {
                        yield return context.UserContext;
                    }
                    else if (metadata.ReturnType.IsInstanceOfType(context.Source))
                    {
                        yield return context.Source;
                    }
                    else if (TypeUtils.IsResolveFieldContextType(metadata.ReturnType))
                    {
                        if (metadata.ReturnType.IsGenericType)
                        {
                            var contextSourceType = metadata.ReturnType.GenericTypeArguments[0];
                            if (context.Source == null || contextSourceType.IsInstanceOfType(context.Source))
                            {
                                yield return Activator.CreateInstance(typeof(ResolveFieldContext<>).MakeGenericType(contextSourceType), context);
                            }
                            else
                            {
                                throw new InvalidOperationException($"Provided context with {contextSourceType.Name} type can not be processed. Context type can be only with {context.Source.GetType().Name} type.");
                            }
                        }
                        else
                        {
                            yield return context;
                        }
                    }
                }
            }
        }


        private class ArgumentMetadata
        {
            public bool IsGraphQlMember { get; }

            public string Name { get; }

            public Type ReturnType { get; }


            public ArgumentMetadata(bool isGraphQlMember, string name, Type returnType)
            {
                IsGraphQlMember = isGraphQlMember;
                ReturnType = returnType;
                Name = name;
            }
        }
    }
}