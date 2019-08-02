using System;
using GraphQL.Types;

namespace GQL.Services.Infra.Providers
{
    public interface IGraphTypeInfoProvider
    {
        void Provide(GraphType graphType, Type type, IServiceProvider serviceProvider);
    }
}