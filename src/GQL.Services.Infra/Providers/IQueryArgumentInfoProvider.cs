using System;
using System.Reflection;
using GraphQL.Types;

namespace GQL.Services.Infra.Providers
{
    public interface IQueryArgumentInfoProvider
    {
        void Provide(QueryArgument queryArgument, ParameterInfo parameterInfo, IServiceProvider serviceProvider);
    }
}