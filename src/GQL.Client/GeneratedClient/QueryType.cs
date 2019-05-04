using System.Collections.Generic;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IQueryType
    {
        ITypeConfigurator<IQueryType, IUserType> User(string id);
        ITypeConfigurator<IQueryType, IUserType> Users();
    }

    public class QueryType : ObjectType, IQueryType
    {
        public ITypeConfigurator<IQueryType, IUserType> User(string id)
        {
            return new ObjectTypeConfigurator<IQueryType, IUserType>(
                this,
                "user",
                new List<Argument>
                {
                    new Argument("id", "ID!", id),
                },
                () => new UserType(),
                IncludeField);
        }

        public ITypeConfigurator<IQueryType, IUserType> Users()
        {
            return new ObjectTypeConfigurator<IQueryType, IUserType>(
                this,
                "users",
                new List<Argument>
                {
                },
                () => new UserType(),
                IncludeField);
        }
    }
}