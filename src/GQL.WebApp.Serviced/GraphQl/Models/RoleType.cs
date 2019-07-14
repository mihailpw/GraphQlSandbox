using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Serviced.GraphQl.Models
{
    public class RoleType : ObjectGraphType<RoleModel>
    {
        public RoleType()
        {
            Name = "Role";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The identificator");
            Field(x => x.Name).Description("The role name");
        }
    }
}