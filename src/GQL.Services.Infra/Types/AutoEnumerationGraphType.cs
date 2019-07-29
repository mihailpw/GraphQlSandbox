using System;
using GQL.Services.Infra.Helpers;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoEnumerationGraphType<T> : EnumerationGraphType<T> where T : Enum
    {
        public AutoEnumerationGraphType()
        {
            var type = typeof(T);

            Name = type.GetNameOrDefault(type.Name);
            Description = type.GetDescription();
            DeprecationReason = type.GetDeprecationReason();
        }


        protected override string ChangeEnumCase(string val)
        {
            return val.ToCamelCase();
        }
    }
}