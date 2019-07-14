using System;
using GQL.DAL.Models;
using GQL.WebApp.Serviced.GraphQl.Models;
using GQL.WebApp.Serviced.Managers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Serviced.GraphQl.Schemas.Users
{
    public class UsersSubscription : ObjectGraphType
    {
        private readonly IServiceProvider _serviceProvider;

        public IUsersManager UsersManager => _serviceProvider.GetService<IUsersManager>();


        public UsersSubscription(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            FieldSubscribeAsync<UserInterface>(
                "addUser",
                resolve: c => new ManagerUserModel { Id = Guid.NewGuid().ToString(), Email = "email" },
                subscribeAsync: async c => await UsersManager.OnAddUserAsync());
        }
    }
}