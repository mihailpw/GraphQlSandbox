using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Typed.Infra
{
    internal class ScopedProvider : IScopedProvider
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