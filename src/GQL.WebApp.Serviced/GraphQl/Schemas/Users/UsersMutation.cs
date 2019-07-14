using System;
using System.Collections.Generic;
using GQL.DAL.Models;
using GQL.WebApp.Serviced.GraphQl.Models;
using GQL.WebApp.Serviced.Managers;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GQL.WebApp.Serviced.GraphQl.Schemas.Users
{
    public class UsersMutation : ObjectGraphType
    {
        private readonly IServiceProvider _serviceProvider;


        public IUsersManager UsersManager => _serviceProvider.GetService<IUsersManager>();


        public UsersMutation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

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