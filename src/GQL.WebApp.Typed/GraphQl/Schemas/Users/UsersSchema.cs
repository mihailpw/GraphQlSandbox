using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersSchema : GraphSchema
    {
        public UsersSchema(UsersQuery query, UsersMutation mutation, UsersSubscription subscription)
            : base(query, mutation, subscription)
        {
            RegisterType<CustomerUserType>();
            RegisterType<ManagerUserType>();
        }
    }
}