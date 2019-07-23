using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra.Core
{
    public static class ProviderContext
    {
        public static IProvider Instance { get; set; }
    }

    public interface IProvider
    {
        object Get(Type type);
        T Get<T>();
    }

    internal class ScopedProvider : IProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public ScopedProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public object Get(Type type)
        {
            return _contextAccessor.HttpContext.RequestServices.GetService(type);
        }

        public T Get<T>()
        {
            return _contextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}