using System;
using GQL.Client.GraphQlClientCore;

namespace GQL.Client.GeneratedClient
{
    public class Query : RootRequestBuilder<Query>

    public interface IUserType
    {
        IUserType Id(bool include = true);
        IUserType Email(bool include = true);
        IUserType Friends(string email, Action<IUserType> setupAction, bool include = true);
    }

    public class UserType : TypeBase, IUserType
    {
        public IUserType Id(bool include = true)
        {
            AddField("id", include);
            return this;
        }

        public IUserType Email(bool include = true)
        {
            AddField("email", include);
            return this;
        }

        public IUserType Friends(string email, Action<IUserType> setupAction, bool include = true)
        {
            AddObject<UserType>(
                "friends",
                b =>
                {
                    b.Arguments.AddArgument("email", "String", email);
                    setupAction(b.Type);
                },
                include);

            return this;
        }
    }
}