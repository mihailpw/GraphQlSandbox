using System.Linq;
using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class UserType : EntityBaseType<UserModel>
    {
        public UserType()
        {
            Name = "User";

            Field(x => x.Name).Description("User name");
            Field(x => x.Email).Description("User email");
            Field<ListGraphType<StringGraphType>>(
                name: nameof(UserModel.Roles),
                description: "User roles",
                resolve: c => c.Source.Roles.Select(r => r.Role.Name));
        }
    }
}