using System.Collections.Generic;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GQL.WebApp.Typed.Infra;
using GQL.WebApp.Typed.Managers;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersMutation : GraphMutation
    {
        private readonly IScopedProvider _scopedProvider;


        public IUsersManager UsersManager => _scopedProvider.Get<IUsersManager>();


        public UsersMutation(IScopedProvider scopedProvider)
        {
            _scopedProvider = scopedProvider;

            FieldAsync<ManagerUserType>(
                "createManager",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ManagerInputType>> { Name = "manager" }),
                resolve: async c =>
                {
                    var manager = c.GetArgument<ManagerUserModel>("manager");
                    return await UsersManager.CreateManagerAsync(manager);
                });
            FieldAsync<ListGraphType<CustomerUserType>>(
                "createCustomers",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<CustomerInputType>> { Name = "customers" }),
                resolve: async c =>
                {
                    var customers = c.GetArgument<List<CustomerUserModel>>("customers");
                    return await UsersManager.CreateCustomersAsync(customers);
                });
        }
    }
}