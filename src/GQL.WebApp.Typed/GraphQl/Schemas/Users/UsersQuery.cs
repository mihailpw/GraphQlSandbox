using System.Linq;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersQuery : GraphQuery
    {
        private readonly AppDbContext _appDbContext;


        public UsersQuery(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            FieldAsync<UserInterface>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>
                    {
                        Name = "id",
                        Description = "User identificator",
                    }),
                resolve: ResolveUserAsync);

            FieldAsync<ListGraphType<UserInterface>>(
                "users",
                resolve: ResolveUsersAsync);

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

        private async Task<object> ResolveCustomersAsync(ResolveFieldContext<object> context)
        {
            var customers = await GetUserModelSet(context)
                .OfType<CustomerUserModel>()
                .ToListAsync();

            return customers;
        }

        private IQueryable<UserModelBase> GetUserModelSet(ResolveFieldContext<object> context)
        {
            IQueryable<UserModelBase> resultQuery = _appDbContext.Set<UserModelBase>();

            if (context.SubFields.Values.TryFindField(nameof(UserModelBase.Roles), out _))
            {
                resultQuery = resultQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
            }

            if (context.SubFields.Values.TryFindField(nameof(UserModelBase.Friends), out _))
            {
                resultQuery = resultQuery.Include(u => u.Friends).ThenInclude(r => r.Friend);
            }

            return resultQuery;
        }
    }
}