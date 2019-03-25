using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class UserRoleType : ObjectGraphType<UserRoleModel>
    {
        public UserRoleType()
        {
            Name = "UserRole";

            Field(x => x.UserId).Description("User id");
            Field(x => x.User, type: typeof(UserType)).Description("User id");
            Field(x => x.RoleId).Description("Role id");
            Field(x => x.Role, type: typeof(RoleType)).Description("Role id");
        }
    }
}