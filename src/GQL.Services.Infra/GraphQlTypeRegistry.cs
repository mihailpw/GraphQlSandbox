using System;
using System.Collections.Generic;
using GQL.Services.Infra.Types;
using GraphQL.Types;

namespace GQL.Services.Infra
{
    public class GraphQlTypeRegistry
    {
        public static readonly GraphQlTypeRegistry Instance = new GraphQlTypeRegistry();

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
            };
        }


        public void RegisterObject(Type type)
        {
            Register(type, typeof(Types.AutoRegisteringObjectGraphType<>).MakeGenericType(type));
        }


        private void Register(Type key, Type value)
        {
            if (_typeToGraphQlTypeMap.TryGetValue(key, out var existingValue))
            {
                if (value == existingValue)
                {
                    return;
                }
                else
                {
                    throw new ArgumentException($"Type {key.Name} already registered.");
                }
            }

            _typeToGraphQlTypeMap.Add(key, value);
        }
    }
}