using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQL.WebApp.Serviced.GraphQl.Infra.Providers;
using GQL.WebApp.Serviced.GraphQl.Infra.Resolvers;
using GQL.WebApp.Serviced.GraphQl.Infra.Types;
using GQL.WebApp.Serviced.Infra;
using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.WebApp.Serviced.GraphQl.Infra
{
    public class ObjectGraphTypeFactory
    {
        private static readonly Dictionary<Type, IGraphType> ScalarGraphTypeFactories = new Dictionary<Type, IGraphType>
        {
            [typeof(bool)] = new BooleanGraphType(),
            [typeof(byte)] = new ByteGraphType(),
            [typeof(DateTime)] = new DateTimeGraphType(),
            [typeof(DateTimeOffset)] = new DateTimeOffsetGraphType(),
            [typeof(decimal)] = new DecimalGraphType(),
            [typeof(float)] = new FloatGraphType(),
            [typeof(Guid)] = new GuidGraphType(),
            [typeof(long)] = new LongGraphType(),
            [typeof(int)] = new IntGraphType(),
            [typeof(sbyte)] = new SByteGraphType(),
            [typeof(short)] = new ShortGraphType(),
            [typeof(string)] = new StringGraphType(),
            [typeof(TimeSpan)] = new TimeSpanMillisecondsGraphType(),
            [typeof(uint)] = new UIntGraphType(),
            [typeof(ulong)] = new ULongGraphType(),
            [typeof(ushort)] = new UShortGraphType(),
            [typeof(Uri)] = new UriGraphType(),
        };

        private readonly IProvider _provider;
        private readonly Dictionary<Type, IGraphType> _createdTypesStorage = new Dictionary<Type, IGraphType>();


        public ObjectGraphTypeFactory(IProvider provider)
        {
            _provider = provider;
        }


        public IGraphType Create(Type type)
        {
            var processingType = type.UnwrapTypeFromTask();

            var nullable = !processingType.IsValueType;
            if (processingType.CheckIfNullable())
            {
                nullable = true;
                processingType = processingType.GenericTypeArguments[0];
            }

            if (!_createdTypesStorage.TryGetValue(processingType, out var result))
            {
                if (ScalarGraphTypeFactories.TryGetValue(processingType, out var scalarGraphType))
                {
                    result = scalarGraphType;
                }
                else if (processingType.IsEnum)
                {
                    result = CreateEnumGraphType(processingType);
                }
                else if (processingType.IsArray)
                {
                    result = new ListGraphType(Create(processingType.GetElementType()));
                }
                else if (processingType.CheckIfEnumerable())
                {
                    result = new ListGraphType(Create(processingType.GetEnumerableElementType()));
                }
                else
                {
                    result = CreateObject(processingType);
                    // throw new NotSupportedException($"Type '{processingType.Name}' can not be presented as {nameof(IGraphType)}");
                }

                result.Name = ProviderUtils.GetName(processingType);
                result.Description = ProviderUtils.GetDescription(processingType);
                result.DeprecationReason = ProviderUtils.GetDeprecationReason(processingType);

                if (!processingType.CheckIfNullable())
                {
                    _createdTypesStorage.Add(type, result);
                }
            }

            if (!nullable)
            {
                result = new NonNullGraphType(result);
            }

            return result;
        }

        private static IGraphType CreateEnumGraphType(Type enumType)
        {
            var enumGraphType = new EnumerationGraphType();
            foreach (var enumValueName in Enum.GetNames(enumType))
            {
                var firstMember = enumType.GetMember(enumValueName, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public).First();

                var enumValueDefinition = new EnumValueDefinition
                {
                    Name = StringUtils.ToCamelCase(ProviderUtils.GetName(firstMember)),
                    Description = ProviderUtils.GetDescription(firstMember),
                    Value = Enum.Parse(enumType, enumValueName),
                    DeprecationReason = ProviderUtils.GetDeprecationReason(firstMember),
                };

                enumGraphType.AddValue(enumValueDefinition);
            }

            return enumGraphType;
        }

        private IGraphType CreateObject(Type type)
        {
            ComplexGraphType<object> result;

            if (ProviderUtils.CheckIfInterfaceType(type))
            {
                result = new InterfaceGraphType();
            }
            else
            {
                result = new ObjectGraphType();
            }

            foreach (var propertyInfo in type.GetProperties())
            {
                result.AddField(CreateFieldType(type, propertyInfo));
            }

            foreach (var methodInfo in type.GetMethods().Where(mi => !mi.IsSpecialName && mi.DeclaringType != typeof(object)))
            {
                result.AddField(CreateFieldType(type, methodInfo));
            }

            return result;
        }

        private FieldType CreateFieldType(Type type, PropertyInfo propertyInfo)
        {
            var fieldType = new FieldType();
            fieldType.Name = ProviderUtils.GetName(propertyInfo);
            fieldType.Description = ProviderUtils.GetDescription(propertyInfo);
            fieldType.DeprecationReason = ProviderUtils.GetDeprecationReason(propertyInfo);

            if (ProviderUtils.TryGetType(propertyInfo, out var propertyType))
            {
                fieldType.Type = propertyType;
            }
            else
            {
                fieldType.ResolvedType = Create(propertyInfo.PropertyType);
            }

            fieldType.Resolver = new PropertyFieldResolver(type, propertyInfo, _provider);

            return fieldType;
        }

        private FieldType CreateFieldType(Type type, MethodInfo methodInfo)
        {
            var graphFieldType = new FieldType();
            graphFieldType.Name = ProviderUtils.GetName(methodInfo);
            graphFieldType.Description = ProviderUtils.GetDescription(methodInfo);
            graphFieldType.DeprecationReason = ProviderUtils.GetDeprecationReason(methodInfo);

            if (ProviderUtils.TryGetType(methodInfo, out var propertyType))
            {
                graphFieldType.Type = propertyType;
            }
            else
            {
                graphFieldType.ResolvedType = Create(methodInfo.ReturnType);
            }

            graphFieldType.Arguments = new QueryArguments();
            foreach (var parameterInfo in methodInfo.GetParameters().Where(pi => !pi.ParameterType.CheckIfResolveFieldContextType()))
            {
                graphFieldType.Arguments.Add(CreateQueryArgument(type, methodInfo, parameterInfo));
            }

            graphFieldType.Resolver = new MethodFieldResolver(type, methodInfo, _provider);

            return graphFieldType;
        }

        private QueryArgument CreateQueryArgument(Type type, MethodInfo methodInfo, ParameterInfo parameterInfo)
        {
            var paramGraphType = Create(parameterInfo.ParameterType);
            var queryArgument = new QueryArgument(paramGraphType);
            queryArgument.Name = ProviderUtils.GetName(parameterInfo);
            queryArgument.Description = ProviderUtils.GetDescription(parameterInfo);
            if (parameterInfo.HasDefaultValue)
            {
                queryArgument.DefaultValue = parameterInfo.DefaultValue;
            }

            return queryArgument;
        }
    }
}