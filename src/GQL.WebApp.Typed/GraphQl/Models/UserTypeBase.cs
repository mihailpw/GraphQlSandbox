using System.Linq;
using GQL.DAL.Models;
using GraphQL.Types;

namespace GQL.WebApp.Typed.GraphQl.Models
{
    public abstract class UserTypeBase<TUser> : EntityBaseType<TUser>
        where TUser : UserModelBase
    {
        protected UserTypeBase()
        {
            Name = "UserInterface";

            Field(x => x.Name).Description("User name");
            Field(x => x.Email).Description("User email");
            Field<ListGraphType<StringGraphType>>(
                name: nameof(UserModelBase.Roles),
                description: "User roles",
                resolve: c => c.Source.Roles.Select(r => r.Role.Name));
            Field<ListGraphType<UserInterface>>(
                name: nameof(UserModelBase.Friends),
                description: "User friends",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "email"}),
                resolve: ResolveFriends);
        }

        private static object ResolveFriends(ResolveFieldContext<TUser> context)
        {
            if (context.Source.Friends == null)
            {
                return null;
            }

            var friends = context
                .Source
                .Friends
                .Select(r => r.Friend);

            var email = context.GetArgument<string>("email");
            if (!string.IsNullOrEmpty(email))
            {
                friends = friends.Where(f => f.Email == email);
            }

            return friends;
        }
    }
}