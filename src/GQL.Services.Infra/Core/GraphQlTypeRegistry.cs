using System;
using System.Collections.Generic;
using System.Linq;
using GQL.Services.Infra.Types;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Core
{
    internal class GraphQlTypeRegistry : IGraphQlTypeRegistry
    {
        private readonly Dictionary<Type, Type> _typeToGraphQlTypeMap;


        public GraphQlTypeRegistry()
        {
            _typeToGraphQlTypeMap = new Dictionary<Type, Type>
            {
                [typeof(bool)] = typeof(BooleanGraphType),
                [typeof(byte)] = typeof(ByteGraphType),
                [typeof(DateTime)] = typeof(DateTimeGraphType),
                [typeof(DateTimeOffset)] = typeof(DateTimeOffsetGraphType),
                [typeof(decimal)] = typeof(DecimalGraphType),
                [typeof(float)] = typeof(FloatGraphType),
                [typeof(Guid)] = typeof(GuidGraphType),
                [typeof(long)] = typeof(LongGraphType),
                [typeof(int)] = typeof(IntGraphType),
                [typeof(sbyte)] = typeof(SByteGraphType),
                [typeof(short)] = typeof(ShortGraphType),
                [typeof(string)] = typeof(StringGraphType),
                [typeof(TimeSpan)] = typeof(TimeSpanMillisecondsGraphType),
                [typeof(uint)] = typeof(UIntGraphType),
                [typeof(ulong)] = typeof(ULongGraphType),
                [typeof(ushort)] = typeof(UShortGraphType),
                [typeof(Uri)] = typeof(UriGraphType),
                [typeof(IdObject)] = typeof(IdGraphType),
            };
        }


        public bool IsRegistered(Type type)
        {
            return _typeToGraphQlTypeMap.ContainsKey(type);
        }

        public Type Resolve(Type type)
        {
            return TryResolve(type, out var graphQlType)
                ? graphQlType
                : throw new InvalidOperationException($"Type {type.Name} is not registered.");
        }

        public bool TryResolve(Type type, out Type graphQlType)
        {
            if (_typeToGraphQlTypeMap.TryGetValue(type, out graphQlType))
            {
                return true;
            }

            if (type.IsEnum)
            {
                graphQlType = typeof(AutoEnumerationGraphType<>).MakeGenericType(type);
                _typeToGraphQlTypeMap.Add(type, graphQlType);
                return true;
            }

            return false;
        }

        public IEnumerable<Type> ResolveAdditional(Type type)
        {
            var graphQlType = Resolve(type);
            foreach (var typeToGraphQlType in _typeToGraphQlTypeMap)
            {
                if (typeToGraphQlType.Value == graphQlType
                    && typeToGraphQlType.Key != type)
                {
                    yield return typeToGraphQlType.Key;
                }
            }
        }

        public Type RegisterInputObject(Type type)
        {
            return Register(type, typeof(AutoInputObjectGraphType<>).MakeGenericType(type));
        }

        public Type RegisterObject(Type type)
        {
            return Register(type, typeof(AutoObjectGraphType<>).MakeGenericType(type));
        }

        public Type RegisterInterface(Type type)
        {
            return Register(type, typeof(AutoInterfaceGraphType<>).MakeGenericType(type));
        }

        public void DirectRegister(Type type, Type graphQlType)
        {
            if (!graphQlType.IsGraphType())
            {
                throw new ArgumentException($"Invalid GraphQL type provided (graphQlType={graphQlType.Name}.", nameof(graphQlType));
            }
            Register(type, graphQlType);
        }


        private Type Register(Type key, Type value)
        {
            if (_typeToGraphQlTypeMap.TryGetValue(key, out var existingValue))
            {
                if (value == existingValue)
                    return existingValue;

                throw new ArgumentException($"Type {key.Name} already registered.");
            }

            _typeToGraphQlTypeMap.Add(key, value);

            return value;
        }
    }
}