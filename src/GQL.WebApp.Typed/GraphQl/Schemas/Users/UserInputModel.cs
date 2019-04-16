using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class UserInputModel : InputObjectGraphType<UserModel>
    {
        public UserInputModel()
        {
            Name = "UserInput";

            Field(m => m.Name, nullable: false);
            Field(m => m.Email, nullable: false);
        }
    }
}