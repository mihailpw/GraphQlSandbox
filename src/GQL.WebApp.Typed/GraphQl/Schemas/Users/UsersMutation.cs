using System.Collections.Generic;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GQL.WebApp.Typed.Managers;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersMutation : GraphMutation
    {
        private readonly IUsersManager _usersManager;


        public UsersMutation(IUsersManager usersManager)
        {
            _usersManager = usersManager;

            FieldAsync<ManagerUserType>(
                "createManager",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ManagerInputType>> { Name = "manager" }),
                resolve: async c =>
                {
                    var manager = c.GetArgument<ManagerUserModel>("manager");
                    return await _usersManager.CreateManagerAsync(manager);
                });
            FieldAsync<ListGraphType<CustomerUserType>>(
                "createCustomers",
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<CustomerInputType>> { Name = "customers" }),
                resolve: async c =>
                {
                    var customers = c.GetArgument<List<CustomerUserModel>>("customers");
                    return await _usersManager.CreateCustomersAsync(customers);
                });
        }
    }
}