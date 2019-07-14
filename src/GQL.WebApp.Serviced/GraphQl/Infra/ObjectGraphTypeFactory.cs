using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GQL.WebApp.Serviced.GraphQl.Infra.Providers;
using GQL.WebApp.Serviced.GraphQl.Infra.Resolvers;
using GQL.WebApp.Serviced.GraphQl.Infra.Types;
using GQL.WebApp.Serviced.Infra;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GQL.WebApp.Serviced.GraphQl.Infra
{
    public class ObjectGraphTypeFactory
    {
        private static readonly Dictionary<Type, Func<IGraphType>> ScalarGraphTypeFactories = new Dictionary<Type, Func<IGraphType>>
        {
            [typeof(bool)] = () => new BooleanGraphType(),
            [typeof(byte)] = () => new ByteGraphType(),
            [typeof(DateTime)] = () => new DateTimeGraphType(),
            [typeof(DateTimeOffset)] = () => new DateTimeOffsetGraphType(),
            [typeof(decimal)] = () => new DecimalGraphType(),
            [typeof(float)] = () => new FloatGraphType(),
            [typeof(Guid)] = () => new GuidGraphType(),
            [typeof(long)] = () => new LongGraphType(),
            [typeof(int)] = () => new IntGraphType(),
            [typeof(sbyte)] = () => new SByteGraphType(),
            [typeof(short)] = () => new ShortGraphType(),
            [typeof(string)] = () => new StringGraphType(),
            [typeof(TimeSpan)] = () => new TimeSpanMillisecondsGraphType(),
            [typeof(uint)] = () => new UIntGraphType(),
            [typeof(ulong)] = () => new ULongGraphType(),
            [typeof(ushort)] = () => new UShortGraphType(),
            [typeof(Uri)] = () => new UriGraphType(),
        };

        private readonly IProvider _provider;


        public ObjectGraphTypeFactory(IProvider provider)
        {
            _provider = provider;
        }


        public IGraphType Create(Type type, bool nullable = false)
        {
            var processingType = type.UnwrapTypeFromTask();

            IGraphType result;

            if (ScalarGraphTypeFactories.TryGetValue(processingType, out var factory))
            {
                result = factory();
            }
            else if (processingType.IsEnum)
            {
                result = CreateEnumGraphType(processingType);
            }
            else if (processingType.IsArray)
            {
                result = new ListGraphType(Create(processingType.GetElementType(), nullable));
            }
            else if (processingType.CheckIfEnumerable())
            {
                result = new ListGraphType(Create(processingType.GetEnumerableElementType(), nullable));
            }
            else if (processingType.CheckIfNullable())
            {
                result = Create(processingType.GenericTypeArguments[0], nullable: true);
            }
            else
            {
                result = CreateObject(processingType);
                // throw new NotSupportedException($"Type '{processingType.Name}' can not be presented as {nameof(IGraphType)}");
            }

            Populate(result, processingType);

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

            foreach (var methodInfo in type.GetMethods().Where(mi => !mi.IsSpecialName))
            {
                result.AddField(CreateFieldType(type, methodInfo));
            }

            return result;
        }

        private FieldType CreateFieldType(Type type, PropertyInfo propertyInfo)
        {
            var fieldType = Populate(new FieldType(), propertyInfo);
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
            var graphFieldType = Populate(new FieldType(), methodInfo);

            if (ProviderUtils.TryGetType(methodInfo, out var propertyType))
            {
                graphFieldType.Type = propertyType;
            }
            else
            {
                graphFieldType.ResolvedType = Create(methodInfo.ReturnType);
            }

            foreach (var parameterInfo in methodInfo.GetParameters().Where(pi => pi.ParameterType != typeof(ResolveFieldContext)))
            {
                var paramGraphType = Create(parameterInfo.ParameterType);
                var queryArgument = Populate(new QueryArgument(paramGraphType), methodInfo);
                queryArgument.DefaultValue = parameterInfo.DefaultValue;
                if (graphFieldType.Arguments == null)
                {
                    graphFieldType.Arguments = new QueryArguments();
                }
                graphFieldType.Arguments.Add(queryArgument);
            }

            graphFieldType.Resolver = new MethodFieldResolver(type, methodInfo, _provider);

            return graphFieldType;
        }

        private static TType Populate<TType>(TType graphInstance, MemberInfo memberInfo)
        {
            if (graphInstance is IFieldType fieldType)
            {
                fieldType.Name = ProviderUtils.GetName(memberInfo);
                fieldType.Description = ProviderUtils.GetDescription(memberInfo);
                fieldType.DeprecationReason = ProviderUtils.GetDeprecationReason(memberInfo);
            }

            if (graphInstance is INamedType namedType)
            {
                namedType.Name = ProviderUtils.GetName(memberInfo);
            }

            if (graphInstance is IGraphType graphType)
            {
                graphType.Description = ProviderUtils.GetDescription(memberInfo);
                graphType.DeprecationReason = ProviderUtils.GetDeprecationReason(memberInfo);
            }

            if (graphInstance is QueryArgument queryArgument)
            {
                queryArgument.Name = ProviderUtils.GetName(memberInfo);
                queryArgument.Description = ProviderUtils.GetDescription(memberInfo);
            }

            return graphInstance;
        }
    }
}