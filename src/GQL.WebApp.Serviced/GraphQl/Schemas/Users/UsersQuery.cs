using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Serviced.GraphQl.Infra;
using GQL.WebApp.Serviced.GraphQl.Models;
using GQL.WebApp.Serviced.Infra;
using GraphQL.Execution;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GQL.WebApp.Serviced.GraphQl.Schemas.Users
{

    [GraphQlObject("User", Description = "User service impl")]
    public class GqlUserType
    {
        [GraphQlField, GraphQlId]
        public string Id { get; set; }

        [GraphQlField]
        public string Name { get; set; }

        [GraphQlField]
        public string Email { get; set; }

        [GraphQlField]
        public UserType Type { get; set; }

        [GraphQlField("roles")]
        public IEnumerable<string> GetRoles(
            ResolveFieldContext context)
        {
            return null;
        }

        [GraphQlField("friends")]
        public IEnumerable<string> GetFriends(
            ResolveFieldContext context,
            [GraphQlParameter] string email = null)
        {
            return null;
        }
    }

    [GraphQlObject("UsersQuery")]
    public class GqlUsersQueryType
    {
        [GraphQlField("user")]
        public async Task<GqlUserType> GetUserAsync(
            ResolveFieldContext context,
            [GraphQlParameter, GraphQlId] string id,
            [GraphQlParameter] UserType? userType)
        {
            return null;
        }

        [GraphQlField("users")]
        public async Task<IEnumerable<GqlUserType>> GetUsersAsync(
            ResolveFieldContext context,
            [GraphQlParameter] UserType? userType)
        {
            return null;
        }

        [GraphQlField("usersCount")]
        public int GetUsersCount(
            ResolveFieldContext context,
            [GraphQlParameter] string type)
        {
            return 0;
        }
    }

    public class UsersQuery : ObjectGraphType
    {
        private readonly IProvider<AppDbContext> _appDbContextProvider;

        public UsersQuery(IProvider<AppDbContext> appDbContextProvider)
        {
            _appDbContextProvider = appDbContextProvider;

            FieldAsync<UserInterface>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>
                    {
                        Name = "id",
                        Description = "User identificator",
                    },
                    new QueryArgument<UserTypeEnum>
                    {
                        Name = "type",
                        Description = "User type",
                    }),
                resolve: ResolveUserAsync);

            FieldAsync<ListGraphType<UserInterface>>(
                "users",
                arguments: new QueryArguments(
                    new QueryArgument<UserTypeEnum>
                    {
                        Name = "type",
                        Description = "GoodGuy, BadGuy, Nobody are allowed",
                    }),
                resolve: ResolveUsersAsync);

            FieldAsync<IntGraphType>(
                "usersCount",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType>
                    {
                        Name = "position",
                        Description = "Can be 'c' for customer or 'm' for manager.",
                    }),
                resolve: ResolveUsersCountAsync);

            FieldAsync<ListGraphType<CustomerUserType>>(
                "customers",
                resolve: ResolveCustomersAsync);
        }


        private async Task<object> ResolveUserAsync(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<string>("id");
            var user = await GetUserModelSet(context)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        private async Task<object> ResolveUsersAsync(ResolveFieldContext<object> context)
        {
            var users = await GetUserModelSet(context)
                .ToListAsync();

            return users;
        }

        private async Task<object> ResolveUsersCountAsync(ResolveFieldContext<object> context)
        {
            context.Arguments.TryGetValue("type", out var type);
            switch ((string) type)
            {
                case "c":
                    return await _appDbContextProvider.Get().Set<CustomerUserModel>().CountAsync();
                case "m":
                    return await _appDbContextProvider.Get().Set<ManagerUserModel>().CountAsync();
                case "":
                case null:
                    return await _appDbContextProvider.Get().Set<UserModelBase>().CountAsync();
                default:
                    context.Errors.Add(new InvalidValueException("type", $"Type '{type}' not found."));
                    return null;
            }
        }

        private async Task<object> ResolveCustomersAsync(ResolveFieldContext<object> context)
        {
            var customers = await GetUserModelSet(context)
                .OfType<CustomerUserModel>()
                .ToListAsync();

            return customers;
        }

        private IQueryable<UserModelBase> GetUserModelSet(ResolveFieldContext<object> context)
        {
            IQueryable<UserModelBase> resultQuery = _appDbContextProvider.Get().Set<UserModelBase>();

            if (context.SubFields.Values.TryFindField(nameof(UserModelBase.Roles), out _))
            {
                resultQuery = resultQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
            }

            if (context.SubFields.Values.TryFindField(nameof(UserModelBase.Friends), out _))
            {
                resultQuery = resultQuery.Include(u => u.Friends).ThenInclude(r => r.Friend);
            }

            if (context.Arguments.TryGetValue(nameof(UserModelBase.Type).ToLower(), out var type)
                && Enum.TryParse<UserType>(type.ToString(), true, out var typeEnum))
            {
                resultQuery = resultQuery.Where(u => u.Type == typeEnum);
            }

            return resultQuery;
        }
    }
}