using System;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GQL.WebApp.Typed.Managers;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersSubscription : GraphSubscription
    {
        public UsersSubscription(IUsersManager usersManager)
        {
            FieldSubscribeAsync<UserInterface>(
                "addUser",
                resolve: c => new ManagerUserModel { Id = Guid.NewGuid().ToString(), Email = "email" },
                subscribeAsync: async c => await usersManager.OnAddUserAsync());
        }
    }
}