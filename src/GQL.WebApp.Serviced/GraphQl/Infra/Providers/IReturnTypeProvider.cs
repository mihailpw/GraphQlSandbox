using System;

namespace GQL.WebApp.Serviced.GraphQl.Infra.Providers
{
    public interface IReturnTypeProvider
    {
        Type ReturnType { get; }
    }
}