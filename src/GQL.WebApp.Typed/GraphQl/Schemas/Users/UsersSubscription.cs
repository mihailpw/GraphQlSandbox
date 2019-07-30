using System;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GQL.WebApp.Typed.Infra;
using GQL.WebApp.Typed.Managers;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersSubscription : GraphSubscription
    {
        private readonly IScopedProvider _scopedProvider;


        private IUsersManager UsersManager => _scopedProvider.Get<IUsersManager>();


        public UsersSubscription(IScopedProvider scopedProvider)
        {
            _scopedProvider = scopedProvider;

            FieldSubscribeAsync<UserInterface>(
                "addUser",
                resolve: c => new ManagerUserModel { Id = Guid.NewGuid().ToString(), Email = "email" },
                subscribeAsync: async c => await UsersManager.OnAddUserAsync());
        }
    }
}