using System;

namespace GQL.Services.Infra.Core
{
    internal interface IScopedProvider
    {
        object Get(Type type);
        T Get<T>();
    }
}