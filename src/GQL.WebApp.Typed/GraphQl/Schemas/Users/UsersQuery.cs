using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Extensions;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GQL.WebApp.Typed.Infra;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersQuery : GraphQuery
    {
        private readonly IScopedProvider _scopedProvider;


        private AppDbContext AppDbContext => _scopedProvider.Get<AppDbContext>();


        public UsersQuery(IScopedProvider scopedProvider)
        {
            _scopedProvider = scopedProvider;

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

            Connection<UserInterface>()
                .Name("usersConnection")
                .Argument<IntGraphType, int>("sss", null)
                .Bidirectional()
                .PageSize(10)
                .ResolveAsync(ResolveUsers);

            this.MethodField<UserInterface>("user2", nameof(ResolveUser2Async));

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

        private async Task<object> ResolveUsers(ResolveConnectionContext<object> context)
        {
            Connection<T> ToConnection<T>(IReadOnlyList<T> values, int totalCount, bool hasPreviousPage, bool hasNextPage, Func<T, string> cursorSelector)
                where T : class
            {
                var firstValue = values.Count > 0 ? values[0] : null;
                var lastValue = values.Count > 0 ? values[values.Count - 1] : null;

                return new Connection<T>
                {
                    TotalCount = totalCount,
                    PageInfo = new PageInfo
                    {
                        HasPreviousPage = hasPreviousPage,
                        HasNextPage = hasNextPage,
                        StartCursor = firstValue != null ? cursorSelector(firstValue) : null,
                        EndCursor = lastValue != null ? cursorSelector(lastValue) : null,
                    },
                    Edges = values.Select(v => new Edge<T> { Cursor = cursorSelector(v), Node = v }).ToList(),
                };
            }

            var users = await GetUserModelSet(context)
                .ToListAsync();

            return ToConnection(users, 200, true, true, m => m.Id);
        }


        private async Task<object> ResolveUserAsync(ResolveFieldContext<object> context)
        {
            var id = context.GetArgument<string>("id");
            var user = await GetUserModelSet(context)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        private async Task<object> ResolveUser2Async(
            ResolveFieldContext<object> context,
            [QueryArgument] string id,
            [QueryArgument(typeof(UserTypeEnum))] UserType? userType = null)
        {
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
                    return await AppDbContext.Set<CustomerUserModel>().CountAsync();
                case "m":
                    return await AppDbContext.Set<ManagerUserModel>().CountAsync();
                case "":
                case null:
                    return await AppDbContext.Set<UserModelBase>().CountAsync();
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
            IQueryable<UserModelBase> resultQuery = AppDbContext.Set<UserModelBase>();

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