using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class ManagerUserInputType : InputObjectGraphType<ManagerUserModel>
    {
        public ManagerUserInputType()
        {
            Name = "ManagerUserInput";

            Field(m => m.Name, nullable: false);
            Field(m => m.Email, nullable: false);
        }
    }
}