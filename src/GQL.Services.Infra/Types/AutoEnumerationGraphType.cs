using System;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoEnumerationGraphType<T> : EnumerationGraphType<T> where T : Enum
    {
        protected override string ChangeEnumCase(string val)
        {
            return val.ToCamelCase();
        }
    }
}