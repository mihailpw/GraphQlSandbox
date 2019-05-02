using System;
using System.Collections.Generic;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IUserType
    {
        IUserType Id(bool include = true);
        IUserType Name(bool include = true);
        IUserType Email(bool include = true);
        IUserType Friends(string email, Action<IUserType> setupAction, bool include = true);
    }

    public class UserType : ObjectType, IUserType
    {
        public IUserType Id(bool include = true)
        {
            if (include)
            {
                IncludeField("id");
            }
            return this;
        }

        public IUserType Name(bool include = true)
        {
            if (include)
            {
                IncludeField("name");
            }
            return this;
        }

        public IUserType Email(bool include = true)
        {
            if (include)
            {
                IncludeField("email");
            }
            return this;
        }

        public IUserType Friends(string email, Action<IUserType> setupAction, bool include = true)
        {
            if (include)
            {
                var type = new UserType();
                setupAction(type);
                IncludeField(
                    "friends",
                    new List<Argument>
                    {
                        new Argument("email", "String", email),
                    },
                    type);
            }
            return this;
        }
    }
}