using System.Linq;
using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Models;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GQL.WebApp.Typed.GraphQl.Query
{
    public class DefaultQuery : ObjectGraphType
    {
        private readonly AppDbContext _appDbContext;


        public DefaultQuery(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            FieldAsync<UserType>(
                "user",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType>
                    {
                        Name = "id",
                        Description = "User identificator",
                    }),
                resolve: ResolveUserAsync);

            FieldAsync<ListGraphType<UserType>>(
                "users",
                resolve: ResolveUsersAsync);
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

        private IQueryable<UserModel> GetUserModelSet(ResolveFieldContext<object> context)
        {
            IQueryable<UserModel> resultQuery = _appDbContext.Set<UserModel>();

            if (context.SubFields.Values.TryFindField(nameof(UserModel.Roles), out var _))
            {
                resultQuery = resultQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
            }

            return resultQuery;
        }
    }
}