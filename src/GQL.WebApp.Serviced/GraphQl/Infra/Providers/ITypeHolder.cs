using System;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Providers
{
    public interface ITypeHolder
    {
        Type Type { get; }
    }
}