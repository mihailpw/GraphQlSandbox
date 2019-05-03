using System.Collections.Generic;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IUserType
    {
        IUserType Id(bool include = true);
        IUserType Name(bool include = true);
        IUserType Email(bool include = true);
        ITypeConfigurator<IUserType, IUserType> Friends(string email = null);
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

        ITypeConfigurator<IUserType, IUserType> IUserType.Friends(string email)
        {
            return new ObjectTypeConfigurator<IUserType, IUserType>(
                this,
                "friends",
                new List<Argument>
                {
                    new Argument("email", "String", email),
                },
                () => new UserType(),
                IncludeField);
        }
    }
}