using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GQL.Services.Infra.Common.Types
{
    internal class LongGraphType : ScalarGraphType
    {
        public override object ParseLiteral(IValue value)
        {
            return (value as LongValue)?.Value;
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(long));
        }

        public override object Serialize(object value)
        {
            return ParseValue(value);
        }
    }
}