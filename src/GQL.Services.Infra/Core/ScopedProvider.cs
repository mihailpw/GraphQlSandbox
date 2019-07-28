using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.Services.Infra.Core
{
    internal class ScopedScopedProvider : IScopedProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public ScopedScopedProvider(IHttpContextAccessor contextAccessor)
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