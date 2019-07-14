using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Serviced.Infra
{
    public interface IProvider
    {
        object Get(Type type);
        T Get<T>();
    }

    public interface IProvider<out T> : IProvider
    {
        T Get();
    }

    public class Provider : IProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;


        public Provider(IHttpContextAccessor contextAccessor)
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

    public class Provider<T> : Provider, IProvider<T>
    {
        public Provider(IHttpContextAccessor contextAccessor)
            : base(contextAccessor)
        {
        }

        public T Get()
        {
            return Get<T>();
        }
    }
}