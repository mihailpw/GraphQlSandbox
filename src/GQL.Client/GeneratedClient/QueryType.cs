using System;
using System.Collections.Generic;
using GQL.Client.Infra;

namespace GQL.Client.GeneratedClient
{
    public interface IQueryType
    {
        IQueryType User(string id, Action<IUserType> setupAction, bool include = true);
        IQueryType Users(Action<IUserType> setupAction, bool include = true);
    }

    public class QueryType : ObjectType, IQueryType
    {
        public IQueryType User(string id, Action<IUserType> setupAction, bool include = true)
        {
            if (include)
            {
                var type = new UserType();
                setupAction(type);
                IncludeField(
                    "user",
                    new List<Argument>
                    {
                        new Argument("id", "ID!", id),
                    },
                    type);
            }
            return this;
        }

        public IQueryType Users(Action<IUserType> setupAction, bool include = true)
        {
            if (include)
            {
                var type = new UserType();
                setupAction(type);
                IncludeField(
                    "users",
                    new List<Argument>
                    {
                    },
                    type);
            }
            return this;
        }
    }
}