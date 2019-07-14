using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Schemas.Users
{
    public class ManagerInputType : InputObjectGraphType<ManagerUserModel>
    {
        public ManagerInputType()
        {
            Name = "ManagerInput";

            Field(m => m.Name, nullable: false);
            Field(m => m.Email, nullable: false);
        }
    }
}