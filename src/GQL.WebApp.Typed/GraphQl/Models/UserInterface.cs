using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public class UserInterface : InterfaceBaseType<UserModelBase>
    {
        public UserInterface()
        {
            Name = "UserInterface";

            Field(x => x.Name).Description("User name");
            Field(x => x.Email).Description("User email");
            Field<ListGraphType<StringGraphType>>(
                name: nameof(UserModelBase.Roles),
                description: "User roles");
            Field<ListGraphType<UserInterface>>(
                name: nameof(UserModelBase.Friends),
                description: "User friends",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "email" }));
            Field<UserTypeEnum>(
                name: nameof(UserModelBase.Type),
                description: "The type of user");
        }
    }
}