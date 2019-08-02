using System;
using GQL.Services.Infra.Core;
using GQL.Services.Infra.Helpers;
using GQL.Services.Infra.Providers;
using GraphQL;
using GraphQL.Types;

namespace GQL.Services.Infra.Types
{
    internal sealed class AutoEnumerationGraphType<T> : EnumerationGraphType<T> where T : Enum
    {
        public AutoEnumerationGraphType()
        {
            var serviceProvider = GlobalContext.ServiceProvider;

            var type = typeof(T);

            type.FindInAttributes<IGraphTypeInfoProvider>()?.Provide(this, type, serviceProvider);
        }


        protected override string ChangeEnumCase(string val)
        {
            return val.ToCamelCase();
        }
    }
}