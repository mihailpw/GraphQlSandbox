using System;

namespace GQL.WebApp.Typed.Infra
{
    public interface IScopedProvider
    {
        object Get(Type type);
        T Get<T>();
    }
}