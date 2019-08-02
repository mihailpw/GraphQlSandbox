using System;
using Microsoft.AspNetCore.Http;

namespace GQL.Services.Infra.Core
{
    public class RequestServicesProvider : IServiceProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RequestServicesProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public object GetService(Type serviceType)
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService(serviceType);
        }
    }
}