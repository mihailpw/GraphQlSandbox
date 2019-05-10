using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Schemas.Users
{
    public class CustomerInputType : InputObjectGraphType<CustomerUserModel>
    {
        public CustomerInputType()
        {
            Name = "CustomerInput";

            Field(m => m.Name, nullable: true);
            Field(m => m.Email, nullable: false).DefaultValue("em");
            Field<ListGraphType<StringGraphType>>(
                name: "roles",
                resolve: _ => new [] { "test1", "test2" });
        }
    }
}