using System.Threading.Tasks;
using GQL.DAL;
using GQL.DAL.Models;
using GQL.WebApp.Typed.GraphQl.Infra;
using GQL.WebApp.Typed.GraphQl.Models;
using GraphQL.Execution;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UsersMutation : GraphMutation
    {
        private readonly AppDbContext _appDbContext;


        public UsersMutation(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            FieldAsync<UserInterface>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputModel>> { Name = "user" }),
                resolve: CreateUserAsync);

            FieldAsync<UserInterface>(
                "createUsers",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<UserInputModel>>>> { Name = "users" }),
                resolve: CreateUserAsync);
        }


        private async Task<object> CreateUserAsync(ResolveFieldContext<object> context)
        {
            var user = context.GetArgument<UserModelBase>("user");

            // context.Errors.Add(new InvalidValueException("user", "bad"));

            await _appDbContext.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
    }
}